﻿using AlzheimerWebAPI.Models;
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
        public async Task<IActionResult> CrearFamiliar([FromBody] Familiares nuevoFamiliar)
        {
            _logger.LogInformation("Creando un nuevo familiar.");

            var familiarCreado = await _familiaresService.CrearFamiliar(nuevoFamiliar);

            return CreatedAtAction(nameof(ObtenerFamiliar), new { id = familiarCreado.IdFamiliar }, familiarCreado);
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

        [HttpPut("familiares/{id}")]
        public async Task<IActionResult> ActualizarFamiliar(Guid id, [FromBody] Familiares familiarActualizado)
        {
            _logger.LogInformation($"Actualizando familiar con ID: {id}");

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
