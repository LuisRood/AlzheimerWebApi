﻿using System.Text.Json;
using Microsoft.AspNetCore.SignalR;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Notifications;
using AlzheimerWebAPI.Repositories;
using NetTopologySuite.Geometries;

namespace AlzheimerWebAPI.Services
{
    public class UbicacionBackgroundService : IHostedService, IDisposable
    {
        private readonly ILogger<UbicacionBackgroundService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHubContext<AlzheimerHub> _hubContext;
        private readonly IServiceScopeFactory _scopeFactory;
        private Timer _timer;
        
        public UbicacionBackgroundService(
            ILogger<UbicacionBackgroundService> logger,
            IHttpClientFactory httpClientFactory,
            IHubContext<AlzheimerHub> hubContext,
            IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _hubContext = hubContext;
            _scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Location Background Service is starting.");

            _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private async void DoWork(object state)
        {
            _logger.LogInformation("Location Background Service is working.");

            var httpClient = _httpClientFactory.CreateClient();

            using (var scope = _scopeFactory.CreateScope())
            {
                var ubicacionesService = scope.ServiceProvider.GetRequiredService<UbicacionesService>();

                try
                {
                    var activeDevices = AlzheimerHub.GetActiveDevices().Keys.ToList();
                    foreach (var mac in activeDevices)
                    {
                        await ObtenerYEnviarUbicacion(httpClient, mac, ubicacionesService);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Error occurred while getting devices: {ex.Message}");
                }
            }
        }
        private async Task ObtenerYEnviarUbicacion(HttpClient httpClient, string mac, UbicacionesService ubicacionesService)
        {
            try
            {
                var response = await httpClient.GetAsync($"http://52.146.12.184:3000/mqtt-request/{mac}");
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    SensorData sensorData = JsonSerializer.Deserialize<SensorData>(responseBody);

                    DateTime fechaHora = (sensorData.Fecha ?? DateTime.MinValue).Date;
                    fechaHora = fechaHora.Add(sensorData.Hora ?? TimeSpan.Zero);
                    // Convertir fechaHora a la zona horaria de México
                    TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                    DateTime fechaHoraMexico = TimeZoneInfo.ConvertTimeFromUtc(fechaHora.ToUniversalTime(), mexicoTimeZone);
                    Ubicaciones ubicaciones = new()
                    {
                        Ubicacion = new Point((double)sensorData.Longitud!, (double)sensorData.Latitud!) { SRID = 4326 },
                        FechaHora = fechaHoraMexico,
                        IdDispositivo = sensorData.Mac,
                    };

                    Ubicaciones ubicacionestemp = await ubicacionesService.ActualizarUbicacionDispositivo(sensorData.Mac, 
                    ubicaciones);
                    if(ubicacionestemp == null)
                    {
                        ubicaciones = await ubicacionesService.CrearUbicacion(ubicaciones);
                    }
                    else
                    {
                        ubicaciones = ubicacionestemp;
                    }
                    if (await ubicacionesService.CheckIfOutsideGeofence(sensorData.Mac,ubicaciones))
                    {
                        _logger.LogInformation("Fuera de zona segura");
                        await _hubContext.Clients.Group(sensorData.Mac).SendAsync("ReceiveLocationOut",
                        sensorData.Mac, sensorData.Latitud, sensorData.Longitud, fechaHoraMexico.ToString());
                    }
                    // Enviar notificación solo a los clientes suscritos al dispositivo específico
                    await _hubContext.Clients.Group(sensorData.Mac).SendAsync("ReceiveLocationUpdate",
                    sensorData.Mac, sensorData.Latitud, sensorData.Longitud, fechaHoraMexico.ToString());
                }
                else
                {
                    // Obtiene la fecha actual del sistema
                    DateTime fechaHora = DateTime.Now.Date;

                    // Añade la hora actual del sistema a la fecha
                    fechaHora = fechaHora.Add(DateTime.Now.TimeOfDay);

                    // Convertir fechaHora a la zona horaria de México
                    TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                    DateTime fechaHoraMexico = TimeZoneInfo.ConvertTimeFromUtc(fechaHora.ToUniversalTime(), mexicoTimeZone);

                    //Aqui esta si no da respuesta el cliente mqtt
                    _logger.LogError($"Failed to get location from external server with status code: {response.StatusCode}");
                    await _hubContext.Clients.Group(mac).SendAsync("ReceiveNotFound", mac, fechaHoraMexico.ToString());
                }
            }
            catch (Exception ex)
            {
                // Obtiene la fecha actual del sistema
                DateTime fechaHora = DateTime.Now.Date;

                // Añade la hora actual del sistema a la fecha
                fechaHora = fechaHora.Add(DateTime.Now.TimeOfDay);

                // Convertir fechaHora a la zona horaria de México
                TimeZoneInfo mexicoTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
                DateTime fechaHoraMexico = TimeZoneInfo.ConvertTimeFromUtc(fechaHora.ToUniversalTime(), mexicoTimeZone);
                _logger.LogError($"Error occurred while getting location: {ex.Message}");
                await _hubContext.Clients.Group(mac).SendAsync("ReceiveNotFound", mac, fechaHoraMexico.ToString());
            }

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
