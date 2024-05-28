using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class MedicamentosController : ControllerBase
    {
        private readonly MedicamentosService _medicamentosService;
        private readonly ILogger<MedicamentosController> _logger;

        public MedicamentosController(MedicamentosService medicamentosService, ILogger<MedicamentosController> logger)
        {
            _medicamentosService = medicamentosService;
            _logger = logger;
        }

        [HttpPost("CrearMedicamento")]
        public async Task<IActionResult> CrearMedicamento([FromBody] Medicamentos nuevoMedicamento)
        {
            _logger.LogInformation("Creando un nuevo medicamento.");

            var medicamentoCreado = await _medicamentosService.CrearMedicamento(nuevoMedicamento);

            return CreatedAtAction(nameof(ObtenerMedicamento), new { id = medicamentoCreado.IdMedicamento }, medicamentoCreado);
        }

        [HttpGet("medicamentos/{id}")]
        public async Task<IActionResult> ObtenerMedicamento(Guid id)
        {
            _logger.LogInformation($"Obteniendo medicamento con ID: {id}");

            var medicamento = await _medicamentosService.ObtenerMedicamento(id);

            if (medicamento == null)
            {
                return NotFound();
            }

            return Ok(medicamento);
        }


        [HttpPut("medicamentos/{id}")]
        public async Task<IActionResult> ActualizarMedicamento(Guid id, [FromBody] MedicamentosDTO medicamentoActualizadoDTO)
        {
            _logger.LogInformation($"Actualizando medicamento con ID: {id}");

            Medicamentos medicamentoActualizado = new()
            {
                IdMedicamento = medicamentoActualizadoDTO.IdMedicamento,
                Nombre = medicamentoActualizadoDTO.Nombre,
                Gramaje = medicamentoActualizadoDTO.Gramaje,
                Descripcion = medicamentoActualizadoDTO.Descripcion,
                IdPaciente = medicamentoActualizadoDTO.IdPaciente
            };

            var medicamento = await _medicamentosService.ActualizarMedicamento(id, medicamentoActualizado);

            if (medicamento == null)
            {
                return NotFound();
            }

            return Ok(medicamento);
        }

        [HttpDelete("medicamentos/{id}")]
        public async Task<IActionResult> EliminarMedicamento(Guid id)
        {
            _logger.LogInformation($"Eliminando medicamento con ID: {id}");

            var medicamentoEliminado = await _medicamentosService.EliminarMedicamento(id);

            if (!medicamentoEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("medicamentospaciente/{id}")]
        public async Task<IActionResult> ObtenerMedicamentoPaciente(string id)
        {
            _logger.LogInformation($"Medicamento del paciente con ID: {id}");

            var medicamentos = await _medicamentosService.ObtenerMedicamentosPaciente(id);

            if(medicamentos.IsNullOrEmpty())
            {
                return NotFound();
            }
            return Ok(medicamentos);
        }
    }
}
