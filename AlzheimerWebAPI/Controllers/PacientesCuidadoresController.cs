using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class PacientesCuidadoresController : ControllerBase
    {
        private readonly PacientesCuidadoresService _pacientesCuidadoresService;
        private readonly CuidadoresService _cuidadoresService;
        private readonly ILogger<PacientesCuidadoresController> _logger;

        public PacientesCuidadoresController(PacientesCuidadoresService pacientesCuidadoresService,CuidadoresService cuidadoresService, ILogger<PacientesCuidadoresController> logger)
        {
            _pacientesCuidadoresService = pacientesCuidadoresService;
            _cuidadoresService = cuidadoresService;
            _logger = logger;
        }

        [HttpPost("CrearRelacion")]
        public async Task<IActionResult> CrearRelacion([FromBody] PacientesCuidadores nuevoPacienteCuidador)
        {
            _logger.LogInformation("Creando una nueva relación paciente-cuidador.");

            var pacienteCuidadorCreado = await _pacientesCuidadoresService.CrearRelacion(nuevoPacienteCuidador);

            return CreatedAtAction(nameof(CrearRelacion), new { id = pacienteCuidadorCreado.IdCuidaPaciente }, pacienteCuidadorCreado);
        }

        [HttpGet("pacientescuidadores/{id}")]
        public async Task<IActionResult> ObtenerRelacion(Guid id)
        {
            _logger.LogInformation($"Obteniendo relación paciente-cuidador con ID: {id}");

            var pacienteCuidador = await _pacientesCuidadoresService.ObtenerRelacion(id);

            if (pacienteCuidador == null)
            {
                return NotFound();
            }

            return Ok(pacienteCuidador);
        }
        [HttpGet("pacientecuidadores/{id}")]
        public async Task<IActionResult> ObtenerRelacionCuidadoresPaciente(string id)
        {
            _logger.LogInformation($"Obteniendo cuidadores del paciente con ID: {id}");

            var cuidadores = await _cuidadoresService.ObtenerPacienteCuidadores(id);

            if (cuidadores == null)
            {
                return NotFound();
            }

            return Ok(cuidadores);
        }

        [HttpPut("pacientescuidadores/{id}")]
        public async Task<IActionResult> ActualizarRelacion(Guid id, [FromBody] PacientesCuidadores pacienteCuidadorActualizado)
        {
            _logger.LogInformation($"Actualizando relación paciente-cuidador con ID: {id}");

            var pacienteCuidador = await _pacientesCuidadoresService.ActualizarRelacion(id, pacienteCuidadorActualizado);

            if (pacienteCuidador == null)
            {
                return NotFound();
            }

            return Ok(pacienteCuidador);
        }

        [HttpDelete("pacientescuidadores/{id}")]
        public async Task<IActionResult> EliminarPacienteCuidador(Guid id)
        {
            _logger.LogInformation($"Eliminando relación paciente-cuidador con ID: {id}");

            var pacienteCuidadorEliminado = await _pacientesCuidadoresService.EliminarRelacion(id);

            if (!pacienteCuidadorEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
