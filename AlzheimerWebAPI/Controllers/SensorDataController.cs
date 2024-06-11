using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Notifications;
using AlzheimerWebAPI.Repositories;
using AlzheimerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class SensorDataController : ControllerBase
    {
        private readonly ILogger<SensorDataController> _logger;
        private readonly HttpClient _httpClient;
        private readonly UbicacionesService _ubicacionesService;
        private readonly IHubContext<AlzheimerHub> _hubContext;
        private readonly NotificacionesService _notificacionesService;
        private readonly DispositivosService _dispositivosService;

        public SensorDataController(ILogger<SensorDataController> logger, HttpClient httpClient, 
            UbicacionesService ubicacionesService,IHubContext<AlzheimerHub> hubContext,NotificacionesService notificacionesService,
            DispositivosService dispositivosService)
        {
            _logger = logger;
            _httpClient = httpClient;
            _ubicacionesService = ubicacionesService;
            _hubContext = hubContext;
            _notificacionesService = notificacionesService;
            _dispositivosService = dispositivosService;
        }

        [HttpPost("ObtenerUbicacionPeriodicamente")]
        [AllowAnonymous]
        public async Task<IActionResult> ObtenerUbicacion([FromBody] SensorData sensorData)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            try
            {
                _logger.LogInformation($"Received data for timestamp {sensorData.Fecha} {sensorData.Hora}");
                Console.WriteLine($"Data received {sensorData}");
                if (sensorData.Mac != null)
                {
                    Console.WriteLine(sensorData.Mac);
                    DateTime fechaHora = (sensorData.Fecha ?? DateTime.MinValue).Date;
                    fechaHora = fechaHora.Add(sensorData.Hora ?? TimeSpan.Zero);
                    if (sensorData.Caida == true)
                    {
                        Dispositivos dispositivo = await _dispositivosService.ObtenerDispositivo(sensorData.Mac);
                        Notificaciones notificacion = new()
                        {
                            Mensaje = $"El dispositivo {sensorData.Mac} ha registrado una caida",
                            Fecha = fechaHora,
                            Hora = fechaHora.TimeOfDay,
                            IdPaciente = dispositivo.Paciente.IdPaciente,
                            IdTipoNotificacion = new Guid("F08E572E-1ED7-4006-B769-3B39B9364D16")
                        };
                        _logger.LogInformation("El Paciente ha caido");
                        await _notificacionesService.CrearNotificacion(notificacion);
                        await _hubContext.Clients.Group(sensorData.Mac).SendAsync("ReceiveFall", sensorData.Mac, fechaHora.ToString());


                    }
                }
                // Procesar los datos según los requisitos
                // Ejemplo de guardar los datos en una base de datos o realizar algún otro procesamiento

                return Ok("Datos recibidos correctamente");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error processing sensor data: {ex.Message}");
                return BadRequest();
            }
        }

        [Authorize]
        [HttpGet("ubicacion/{mac}")]
        public async Task<IActionResult> ObtenerUbicacionPeriodicamente(string mac)
        {
            _logger.LogInformation($"Obteniendo ubicación para el dispositivo con MAC: {mac}");

            try
            {
                if (string.IsNullOrEmpty(mac))
                {
                    _logger.LogError("No se proporcionó el parámetro 'mac'.");
                    return BadRequest("No se proporcionó el parámetro 'mac'.");
                }

                // Hacer una solicitud GET al servidor externo
                var externalServerUrl = $"http://52.146.12.184:3000/mqtt-request/{mac}";
                var responseFromExternalServer = await _httpClient.GetAsync(externalServerUrl);

                if (responseFromExternalServer.IsSuccessStatusCode)
                {
                    var responseBody = await responseFromExternalServer.Content.ReadAsStringAsync();
                    _logger.LogInformation($"Respuesta del servidor externo: {responseBody}");
                    SensorData sensorData = JsonSerializer.Deserialize<SensorData>(responseBody);
                    DateTime fechaHora = (sensorData.Fecha ?? DateTime.MinValue).Date;
                    fechaHora = fechaHora.Add(sensorData.Hora ?? TimeSpan.Zero);
                    Ubicaciones ubicaciones = new()
                    {
                        Ubicacion = new Point((double)sensorData.Longitud!, (double)sensorData.Latitud!) { SRID = 4326 },
                        FechaHora = fechaHora,
                        IdDispositivo = sensorData.Mac,
                    };
                    Console.WriteLine(ubicaciones.Ubicacion.ToString());
                    ubicaciones = await _ubicacionesService.ActualizarUbicacionDispositivo(sensorData.Mac, ubicaciones);

                    // Aquí puedes agregar lógica adicional si es necesario
                    //if(await _ubicacionesService.CheckIfOutsideGeofence(sensorData.Mac, ubicaciones))
                    //{
                    //    Console.WriteLine("Salio de zona segura");
                        //await _hubContext.Clients.All.SendAsync("ReceiveMessage", sensorData.Mac, sensorData.Latitud, sensorData.Longitud);
                    //await _hubContext.Clients.All.SendAsync("ReceiveLocationUpdate", sensorData.Mac, sensorData.Latitud, sensorData.Longitud);
                    //     // Lógica para notificaciones si el dispositivo está fuera de la zona segura
                    //}

                    UbicacionesDTO ubicacionesDTO = new UbicacionesDTO(ubicaciones);
                    return Ok(ubicacionesDTO);
                }
                else
                {
                    _logger.LogError($"La solicitud GET al servidor externo falló con código de estado: {responseFromExternalServer.StatusCode}");
                    Ubicaciones ubicacion = await _ubicacionesService.ObtenerUbicacionPorDispositivo(mac);
                    UbicacionesDTO ubicacionDTO = new(ubicacion);
                    return StatusCode((int)responseFromExternalServer.StatusCode, ubicacionDTO);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error al procesar la solicitud: {ex.Message}");
                return StatusCode(500, "Error al procesar la solicitud");
            }
        }
    }
}
