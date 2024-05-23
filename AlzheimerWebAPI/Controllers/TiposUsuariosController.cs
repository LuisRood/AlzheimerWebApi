using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class TiposUsuariosController : ControllerBase
    {
        private readonly TiposUsuariosService _tiposUsuariosService;
        private readonly ILogger<TiposUsuariosController> _logger;

        public TiposUsuariosController(TiposUsuariosService tiposUsuariosService, ILogger<TiposUsuariosController> logger)
        {
            _tiposUsuariosService = tiposUsuariosService;
            _logger = logger;
        }

        [HttpPost("CrearTipoUsuario")]
        public async Task<IActionResult> CrearTipoUsuario([FromBody] TiposUsuarios nuevoTipoUsuario)
        {
            _logger.LogInformation("Creando un nuevo tipo de usuario.");

            var tipoUsuarioCreado = await _tiposUsuariosService.CrearTipoUsuario(nuevoTipoUsuario);

            return CreatedAtAction(nameof(ObtenerTipoUsuarioPorTipo), new { tipo = tipoUsuarioCreado.TipoUsuario }, tipoUsuarioCreado);
        }

        [HttpGet("tiposusuarios/{tipo}")]
        public async Task<IActionResult> ObtenerTipoUsuarioPorTipo(string tipo)
        {
            _logger.LogInformation($"Obteniendo tipo de usuario: {tipo}");

            var tipoUsuario = await _tiposUsuariosService.ObtenerTiposUsuarioTipo(tipo);

            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return Ok(tipoUsuario);
        }

        [HttpPut("tiposusuarios/{id}")]
        public async Task<IActionResult> ActualizarTipoUsuario(Guid id, [FromBody] TiposUsuarios tipoUsuarioActualizado)
        {
            _logger.LogInformation($"Actualizando tipo de usuario con ID: {id}");

            var tipoUsuario = await _tiposUsuariosService.ActualizarTipoUsuario(id, tipoUsuarioActualizado);

            if (tipoUsuario == null)
            {
                return NotFound();
            }

            return Ok(tipoUsuario);
        }

        [HttpDelete("tiposusuarios/{id}")]
        public async Task<IActionResult> EliminarTipoUsuario(Guid id)
        {
            _logger.LogInformation($"Eliminando tipo de usuario con ID: {id}");

            var tipoUsuarioEliminado = await _tiposUsuariosService.EliminarTipoUsuario(id);

            if (!tipoUsuarioEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
