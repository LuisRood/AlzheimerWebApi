using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
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
    public class FamiliaresController : ControllerBase
    {
        private readonly FamiliaresService _familiaresService;
        private readonly ILogger<FamiliaresController> _logger;

        public FamiliaresController(FamiliaresService familiaresService, ILogger<FamiliaresController> logger)
        {
            _familiaresService = familiaresService;
            _logger = logger;
        }

        [HttpPost("CrearFamiliar")]
        public async Task<IActionResult> CrearFamiliar()//[FromBody] FamiliaresDTO nuevoFamiliarDTO)
        {
            _logger.LogInformation("Creando un nuevo familiar.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevoFamiliar = JsonSerializer.Deserialize<Familiares>(requestBody);

            //var nuevoFamiliar = new Familiares(nuevoFamiliarDTO);
            var familiarCreado = await _familiaresService.CrearFamiliar(nuevoFamiliar);

            return Ok(familiarCreado);
            //return CreatedAtAction(nameof(ObtenerFamiliar), new { id = familiarCreado.IdFamiliar }, familiarCreado);
        }

        [HttpGet("familiares/{id}")]
        public async Task<IActionResult> ObtenerFamiliar(Guid id)
        {
            _logger.LogInformation($"Obteniendo familiar con ID: {id}");

            var familiar = await _familiaresService.ObtenerFamiliar(id);

            if (familiar == null)
            {
                return NotFound();
            }

            return Ok(familiar);
        }

        [HttpGet("todosfamiliares/")]
        public async Task<IActionResult> ObtenerTodo()
        {
            _logger.LogInformation($"Obteniendo todos los Familiares");

            var familiares = await _familiaresService.ObtenerTodosFamiliares();
            if (familiares.IsNullOrEmpty())
            {
                return NotFound();
            }

            return Ok(familiares);
        }


        [HttpPut("familiares/{id}")]
        public async Task<IActionResult> ActualizarFamiliar(Guid id)//, [FromBody] FamiliaresDTO familiarActualizadoDTO)
        {
            _logger.LogInformation($"Actualizando familiar con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var familiarActualizado = JsonSerializer.Deserialize<Familiares>(requestBody);

            //var familiarActualizado = new Familiares(familiarActualizadoDTO);
            var familiar = await _familiaresService.ActualizarFamiliar(id, familiarActualizado);

            if (familiar == null)
            {
                return NotFound();
            }

            return Ok(familiar);
        }

        [HttpDelete("familiares/{id}")]
        public async Task<IActionResult> EliminarFamiliar(Guid id)
        {
            _logger.LogInformation($"Eliminando familiar con ID: {id}");

            var familiarEliminado = await _familiaresService.EliminarFamiliar(id);

            if (!familiarEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
