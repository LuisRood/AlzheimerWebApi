using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class PacientesFamiliaresController : ControllerBase
    {
        private readonly PacientesFamiliaresService _pacientesFamiliaresService;
        private readonly ILogger<PacientesFamiliaresController> _logger;

        public PacientesFamiliaresController(PacientesFamiliaresService pacientesFamiliaresService, ILogger<PacientesFamiliaresController> logger)
        {
            _pacientesFamiliaresService = pacientesFamiliaresService;
            _logger = logger;
        }

        [HttpPost("CrearRelacionFamiliares")]
        public async Task<IActionResult> CrearRelacion()//[FromBody] PacientesFamiliares nuevaRelacion)
        {
            _logger.LogInformation("Creando una nueva relación Pacientes-Familiares.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevaRelacion = JsonSerializer.Deserialize<PacientesFamiliares>(requestBody);

            var relacionCreada = await _pacientesFamiliaresService.CrearRelacion(nuevaRelacion);

            PacientesFamiliaresDTO pacientefamiliar = new(relacionCreada);
            return Ok(pacientefamiliar);
            //return CreatedAtAction(nameof(ObtenerRelacion), new { id = relacionCreada.IdPacienteFamiliar }, relacionCreada);
        }

        [HttpGet("pacientesfamiliares/{id}")]
        public async Task<IActionResult> ObtenerRelacion(string id)
        {
            _logger.LogInformation($"Obteniendo relación Pacientes-Familiares con ID: {id}");

            var relacion = await _pacientesFamiliaresService.ObtenerRelacion(id);

            if (relacion == null)
            {
                return NotFound();
            }
            PacientesFamiliaresDTO pacientefamiliar = new(relacion);
            return Ok(pacientefamiliar);
        }

        [HttpGet("pacientefamiliares/{id}")]
        public async Task<IActionResult> ObtenerFamiliares(string id)
        {
            _logger.LogInformation($"Obteniendo familiares del paciente con ID: {id}");

            var familiares = await _pacientesFamiliaresService.ObtenerFamiliares(id);

            if (familiares == null || !familiares.Any())
            {
                return NotFound();
            }

            return Ok(familiares);
        }

        [HttpPut("pacientesfamiliares/{id}")]
        public async Task<IActionResult> ActualizarRelacion(Guid id)//, [FromBody] PacientesFamiliares relacionActualizada)
        {
            _logger.LogInformation($"Actualizando relación Pacientes-Familiares con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var relacionActualizada = JsonSerializer.Deserialize<PacientesFamiliares>(requestBody);

            var relacion = await _pacientesFamiliaresService.ActualizarRelacion(id, relacionActualizada);

            if (relacion == null)
            {
                return NotFound();
            }

            return Ok(relacion);
        }

        [HttpDelete("pacientesfamiliares/{id}")]
        public async Task<IActionResult> EliminarRelacion(Guid id)
        {
            _logger.LogInformation($"Eliminando relación Pacientes-Familiares con ID: {id}");

            var relacionEliminada = await _pacientesFamiliaresService.EliminarRelacion(id);

            if (!relacionEliminada)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
