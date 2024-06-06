using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class PacientesController : ControllerBase
    {
        private readonly PacientesService _pacientesService;
        private readonly ILogger<PacientesController> _logger;

        public PacientesController(PacientesService pacientesService, ILogger<PacientesController> logger)
        {
            _pacientesService = pacientesService;
            _logger = logger;
        }

        [HttpPost("CrearPaciente")]
        public async Task<IActionResult> CrearPaciente()//[FromBody] Pacientes nuevoPaciente)
        {
            _logger.LogInformation("Creando un nuevo paciente.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevoPaciente = JsonSerializer.Deserialize<Pacientes>(requestBody);

            var pacienteCreado = await _pacientesService.CrearPaciente(nuevoPaciente);

            var pacienteCreadoDTO = new PacientesDTO(pacienteCreado);

            return Ok(pacienteCreadoDTO);
            //return CreatedAtAction(nameof(ObtenerPaciente), new { id = pacienteCreadoDTO.IdPaciente }, pacienteCreadoDTO);
        }

        [HttpGet("pacientes/{id}")]
        public async Task<IActionResult> ObtenerPaciente(Guid id)
        {
            _logger.LogInformation($"Obteniendo paciente con ID: {id}");

            var paciente = await _pacientesService.ObtenerPaciente(id);

            if (paciente == null)
            {
                return NotFound();
            }
            var pacienteDTO = new PacientesDTO(paciente);

            return Ok(pacienteDTO);
        }

        [HttpGet("pacienteslista/{id}")]
        public async Task<IActionResult> ObtenerPacientes(Guid id)
        {
            _logger.LogInformation($"Obteniendo lista de pacientes del usuario con ID: {id}");

            var pacientes = await _pacientesService.ObtenerPacientes(id);

            if (pacientes == null || !pacientes.Any())
            {
                return NotFound();
            }
            List<PacientesDTO> pacientesDTO = pacientes.Select(p => new PacientesDTO(p)).ToList();
            return Ok(pacientesDTO);
        }
        [HttpGet("todospacientes")]
        public async Task<IActionResult> ObtenerTodo()
        {
            _logger.LogInformation($"Obteniendo todos los Familiares");

            var pacientes = await _pacientesService.ObtenerTodosPacientes();
            if (pacientes.IsNullOrEmpty())
            {
                return NotFound();
            }

            List<PacientesDTO> pacientesDTO = pacientes.Select(p => new PacientesDTO(p)).ToList();
            return Ok(pacientesDTO);
        }

        [HttpPut("pacientes/{id}")]
        public async Task<IActionResult> ActualizarPaciente(string id)//, [FromBody] PacientesDTO pacienteActualizadoDTO)
        {
            _logger.LogInformation($"Actualizando paciente con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var pacienteActualizado = JsonSerializer.Deserialize<Pacientes>(requestBody);

            /*var pacienteActualizado = new Pacientes
            {
                IdPaciente = pacienteActualizadoDTO.IdPaciente,
                IdDispositivo = pacienteActualizadoDTO.IdDispositivo,
                IdPersona = pacienteActualizadoDTO.IdPersona
            };*/

            var paciente = await _pacientesService.ActualizarPaciente(id, pacienteActualizado);

            if (paciente == null)
            {
                return NotFound();
            }

            var pacienteDTO = new PacientesDTO(paciente);
            return Ok(pacienteDTO);
        }

        [HttpDelete("pacientes/{id}")]
        public async Task<IActionResult> EliminarPaciente(string id)
        {
            _logger.LogInformation($"Eliminando paciente con ID: {id}");

            var pacienteEliminado = await _pacientesService.EliminarPaciente(id);

            if (!pacienteEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
