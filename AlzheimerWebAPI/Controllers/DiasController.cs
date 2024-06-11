using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class DiasController : ControllerBase
    {
        private readonly DiasService _diasService;
        private readonly ILogger<DiasController> _logger;

        public DiasController(DiasService diasService, ILogger<DiasController> logger)
        {
            _diasService = diasService;
            _logger = logger;
        }

        [HttpPost("CrearDia")]
        public async Task<IActionResult> CrearDia()
        {
            _logger.LogInformation("Creando un nuevo día.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevoDia = JsonSerializer.Deserialize<Dias>(requestBody);

            var diaCreado = await _diasService.CrearDia(nuevoDia);
            return Ok(diaCreado);
        }

        [HttpGet("dias/{id}")]
        public async Task<IActionResult> ObtenerDia(Guid id)
        {
            _logger.LogInformation($"Obteniendo día con ID: {id}");

            var dia = await _diasService.ObtenerDia(id);

            if (dia == null)
            {
                return NotFound();
            }

            return Ok(dia);
        }

        [HttpPut("dias/{id}")]
        public async Task<IActionResult> ActualizarDia(Guid id)
        {
            _logger.LogInformation($"Actualizando día con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var diaActualizado = JsonSerializer.Deserialize<Dias>(requestBody);

            var dia = await _diasService.ActualizarDia(id, diaActualizado);

            if (dia == null)
            {
                return NotFound();
            }

            return Ok(dia);
        }

        [HttpDelete("dias/{id}")]
        public async Task<IActionResult> EliminarDia(Guid id)
        {
            _logger.LogInformation($"Eliminando día con ID: {id}");

            var diaEliminado = await _diasService.EliminarDia(id);

            if (!diaEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
