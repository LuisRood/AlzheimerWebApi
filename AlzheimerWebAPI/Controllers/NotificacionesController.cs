using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AlzheimerWebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
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
    }
}
