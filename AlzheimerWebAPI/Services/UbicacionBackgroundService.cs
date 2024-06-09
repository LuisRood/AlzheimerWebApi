using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Notifications;
using AlzheimerWebAPI.Repositories;
using NetTopologySuite.Geometries;
using System.Collections.Concurrent;

namespace AlzheimerWebAPI.Services
{
    public class UbicacionBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger<UbicacionBackgroundService> _logger;
        private readonly HttpClient _httpClient;
        private readonly IHubContext<AlzheimerHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;

        private static readonly Guid SafeZoneNotificationId = new("1C54A3D4-0136-4AE9-AC44-51151254C734");
        private static readonly Guid ConnectionLostNotificationId = new("41706CF7-E7EB-45ED-AF7A-7637EE86D499");

        private readonly ConcurrentDictionary<string, DateTime> _ultimaNotificacion;

        public UbicacionBackgroundService(
            ILogger<UbicacionBackgroundService> logger,
            IHttpClientFactory httpClientFactory,
            IHubContext<AlzheimerHub> hubContext,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _httpClient = httpClientFactory.CreateClient();
            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
            _ultimaNotificacion = new ConcurrentDictionary<string, DateTime>();
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Location Background Service is starting.");

            _timer = new Timer(async _ => await DoWork(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async Task DoWork()
        {
            _logger.LogInformation("Location Background Service is working.");

            using (var scope = _scopeFactory.CreateScope())
            {
                var ubicacionesService = scope.ServiceProvider.GetRequiredService<UbicacionesService>();
                var notificacionesService = scope.ServiceProvider.GetRequiredService<NotificacionesService>();
                var tiposNotificacionesService = scope.ServiceProvider.GetRequiredService<TiposNotificacionesService>();
                var dispositivosService = scope.ServiceProvider.GetRequiredService<DispositivosService>();

                try
                {
                    var activeDevices = AlzheimerHub.GetActiveDevices().Keys.ToList();
                    var tasks = activeDevices.Select(mac =>
                        ObtenerYEnviarUbicacion(_httpClient, mac, ubicacionesService, tiposNotificacionesService, notificacionesService, dispositivosService)
                    ).ToList();

                    await Task.WhenAll(tasks);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while getting devices: {ex.Message}");
                }
            }
        }

        private async Task CrearNotificacion(TiposNotificacionesService tiposNotificacionesService,
            NotificacionesService notificacionesService, DispositivosService dispositivosService,
            string mac, DateTime fechaHora, Guid id, string mensaje)
        {
            try
            {
                Dispositivos dispositivo = await dispositivosService.ObtenerDispositivo(mac);
                TiposNotificaciones tiposNotificaciones = await tiposNotificacionesService.ObtenerNotificacion(id);
                Notificaciones notificacion = new()
                {
                    Mensaje = mensaje,
                    Fecha = fechaHora,
                    Hora = fechaHora.TimeOfDay,
                    IdPaciente = dispositivo.Paciente.IdPaciente,
                    IdTipoNotificacion = tiposNotificaciones.IdTipoNotificacion
                };
                await notificacionesService.CrearNotificacion(notificacion);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error registrando la notificación: {ex.Message}");
            }
        }

        private static DateTime ConvertToMexicoTime(DateTime utcTime)
        {
            TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            return TimeZoneInfo.ConvertTimeFromUtc(utcTime, mexicoTimeZone);
        }

        private async Task ObtenerYEnviarUbicacion(HttpClient httpClient, string mac, UbicacionesService ubicacionesService,
            TiposNotificacionesService tiposNotificacionesService, NotificacionesService notificacionesService,
            DispositivosService dispositivosService)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://52.146.12.184:3000/mqtt-request/{mac}");
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    SensorData sensorData = JsonSerializer.Deserialize<SensorData>(responseBody);

                    DateTime fechaHora = (sensorData.Fecha ?? DateTime.MinValue).Date.Add(sensorData.Hora ?? TimeSpan.Zero);
                    DateTime fechaHoraMexico = ConvertToMexicoTime(fechaHora);
                    Ubicaciones ubicaciones = new()
                    {
                        Ubicacion = new Point((double)sensorData.Longitud!, (double)sensorData.Latitud!) { SRID = 4326 },
                        FechaHora = fechaHoraMexico,
                        IdDispositivo = sensorData.Mac,
                    };

                    Ubicaciones ubicacionestemp = await ubicacionesService.ActualizarUbicacionDispositivo(sensorData.Mac, ubicaciones);
                    if (ubicacionestemp == null)
                    {
                        ubicaciones = await ubicacionesService.CrearUbicacion(ubicaciones);
                    }
                    else
                    {
                        ubicaciones = ubicacionestemp;
                    }

                    if (await ubicacionesService.CheckIfOutsideGeofence(sensorData.Mac, ubicaciones))
                    {
                        _logger.LogInformation("Fuera de zona segura");
                        await RegistrarNotificacionSiEsNecesario(tiposNotificacionesService, notificacionesService, dispositivosService,
                            mac, fechaHoraMexico, SafeZoneNotificationId,
                            $"El dispositivo {mac} salió de la zona segura");
                        await _hubContext.Clients.Group(sensorData.Mac).SendAsync("ReceiveLocationOut",
                        sensorData.Mac, sensorData.Latitud, sensorData.Longitud, fechaHoraMexico.ToString());
                    }

                    // Enviar notificación solo a los clientes suscritos al dispositivo específico
                    await _hubContext.Clients.Group(sensorData.Mac).SendAsync("ReceiveLocationUpdate",
                    sensorData.Mac, sensorData.Latitud, sensorData.Longitud, fechaHoraMexico.ToString());
                }
                else
                {
                    DateTime fechaHoraMexico = ConvertToMexicoTime(DateTime.UtcNow);

                    _logger.LogError($"Failed to get location from external server with status code: {response.StatusCode}");
                    await RegistrarNotificacionSiEsNecesario(tiposNotificacionesService, notificacionesService, dispositivosService,
                        mac, fechaHoraMexico, ConnectionLostNotificationId,
                        $"El dispositivo {mac} se quedó sin conexión");
                    await _hubContext.Clients.Group(mac).SendAsync("ReceiveNotFound", mac, fechaHoraMexico.ToString());
                }
            }
            catch (Exception ex)
            {
                DateTime fechaHoraMexico = ConvertToMexicoTime(DateTime.UtcNow);

                _logger.LogError($"Error occurred while getting location: {ex.Message}");
                await RegistrarNotificacionSiEsNecesario(tiposNotificacionesService, notificacionesService, dispositivosService,
                    mac, fechaHoraMexico, ConnectionLostNotificationId,
                    $"El dispositivo {mac} se quedó sin conexión");
                await _hubContext.Clients.Group(mac).SendAsync("ReceiveNotFound", mac, fechaHoraMexico.ToString());
            }
        }

        private async Task RegistrarNotificacionSiEsNecesario(TiposNotificacionesService tiposNotificacionesService,
            NotificacionesService notificacionesService, DispositivosService dispositivosService,
            string mac, DateTime fechaHora, Guid id, string mensaje)
        {
            if (_ultimaNotificacion.TryGetValue(mac, out DateTime ultimaFechaHora))
            {
                if ((fechaHora - ultimaFechaHora).TotalMinutes < 1)
                {
                    return; // No registrar notificación si no ha pasado al menos un minuto
                }
            }

            await CrearNotificacion(tiposNotificacionesService, notificacionesService, dispositivosService, mac, fechaHora, id, mensaje);
            _ultimaNotificacion[mac] = fechaHora; // Actualizar la última hora de notificación
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Location Background Service is stopping.");

            _timer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
