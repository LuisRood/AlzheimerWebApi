using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using AlzheimerWebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;

namespace AlzheimerWebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class NotificacionesController : ControllerBase
    {
        private readonly NotificacionesService _notificacionesService;
        private readonly ILogger<CuidadoresController> _logger;

        public NotificacionesController(NotificacionesService notificacionesService, ILogger<CuidadoresController> logger)
        {
            _notificacionesService = notificacionesService;
            _logger = logger;
        }

        [HttpGet("notificaciones/{id}")]
        public async Task<IActionResult> ObtenerNotificaciones(Guid id)
        {
            _logger.LogInformation($"Obteniendo notificaciones del usuario con ID: {id}");

            List<Notificaciones> notificaciones = await _notificacionesService.ObtenerNotificacionesParaUsuario(id);

            if (notificaciones.IsNullOrEmpty())
            {
                return NotFound();
            }
            List<NotificacionesDTO> notificacionesDTO = notificaciones.Select(n => new NotificacionesDTO(n)).ToList();

            return Ok(notificacionesDTO);
        }

        [HttpPost("notificaciones")]
        public async Task<IActionResult> CrearNotificaciones(Guid id)
        {
            _logger.LogInformation("Creando un nuevo medicamento.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevaNotificacion = JsonSerializer.Deserialize<Notificaciones>(requestBody);

            var notificacionCreada = await _notificacionesService.CrearNotificacion(nuevaNotificacion);

            NotificacionesDTO notificacionCreadaDTO = new(notificacionCreada);

            return Ok(notificacionCreadaDTO);
        }
        [HttpGet("notificacionesmed/{id}")]
        public async Task<IActionResult> ObtenerNotificacionesMed(Guid id)
        {
            _logger.LogInformation($"Obteniendo notificaciones del usuario con ID: {id}");

            List<Notificaciones> notificaciones = await _notificacionesService.ObtenerNotificacionesParaUsuarioMed(id);

            if (notificaciones.IsNullOrEmpty())
            {
                return NotFound();
            }
            List<NotificacionesDTO> notificacionesDTO = notificaciones.Select(n => new NotificacionesDTO(n)).ToList();

            return Ok(notificacionesDTO);
        }
    }
}
