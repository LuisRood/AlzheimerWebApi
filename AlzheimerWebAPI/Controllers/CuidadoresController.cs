using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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
    public class CuidadoresController : ControllerBase
    {
        private readonly CuidadoresService _cuidadoresService;
        private readonly ILogger<CuidadoresController> _logger;

        public CuidadoresController(CuidadoresService cuidadoresService, ILogger<CuidadoresController> logger)
        {
            _cuidadoresService = cuidadoresService;
            _logger = logger;
        }

        [HttpPost("CrearCuidador")]
        public async Task<IActionResult> CrearCuidador()//[FromBody] CuidadoresDTO nuevoCuidadorDTO)
        {
            _logger.LogInformation("Creando un nuevo cuidador.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevoCuidador = JsonSerializer.Deserialize<Cuidadores>(requestBody);

            //var nuevoCuidador = new Cuidadores(nuevoCuidadorDTO);
            var cuidadorCreado = await _cuidadoresService.CrearCuidador(nuevoCuidador);
            return Ok(cuidadorCreado);
            //return CreatedAtAction(nameof(ObtenerCuidador), new { id = cuidadorCreado.IdCuidador }, cuidadorCreado);
        }

        [HttpGet("todoscuidadores/")]
        public async Task<IActionResult> ObtenerTodo()
        {
            _logger.LogInformation($"Obteniendo todos los Cuidadores");

            var cuidadores = await _cuidadoresService.ObtenerTodosCuidadores();
            if (cuidadores.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(cuidadores);
        }

        [HttpGet("cuidadores/{id}")]
        public async Task<IActionResult> ObtenerCuidador(Guid id)
        {
            _logger.LogInformation($"Obteniendo cuidador con ID: {id}");

            var cuidador = await _cuidadoresService.ObtenerCuidador(id);

            if (cuidador == null)
            {
                return NotFound();
            }

            return Ok(cuidador);
        }

        [HttpPut("cuidadores/{id}")]
        public async Task<IActionResult> ActualizarCuidador(Guid id)//, [FromBody] CuidadoresDTO cuidadorActualizadoDTO)
        {
            _logger.LogInformation($"Actualizando cuidador con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var cuidadorActualizado = JsonSerializer.Deserialize<Cuidadores>(requestBody);

            //var cuidadorActualizado = new Cuidadores(cuidadorActualizadoDTO);
            var cuidador = await _cuidadoresService.ActualizarCuidador(id, cuidadorActualizado);

            if (cuidador == null)
            {
                return NotFound();
            }

            return Ok(cuidador);
        }

        [HttpDelete("cuidadores/{id}")]
        public async Task<IActionResult> EliminarCuidador(Guid id)
        {
            _logger.LogInformation($"Eliminando cuidador con ID: {id}");

            var cuidadorEliminado = await _cuidadoresService.EliminarCuidador(id);

            if (!cuidadorEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
