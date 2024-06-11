﻿using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class PersonasController : ControllerBase
    {
        private readonly PersonasService _personasService;
        private readonly ILogger<PersonasController> _logger;

        public PersonasController(PersonasService personasService, ILogger<PersonasController> logger)
        {
            _personasService = personasService;
            _logger = logger;
        }

        [HttpPost("CrearPersona")]
        public async Task<IActionResult> CrearPersona()//[FromBody] Personas nuevaPersona)
        {
            _logger.LogInformation("Creando una nueva persona.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevaPersona = JsonSerializer.Deserialize<Personas>(requestBody);

            var personaCreada = await _personasService.CrearPersona(nuevaPersona);
            PersonasDTO personaCreadaDTO = new(personaCreada);
            return Ok(personaCreadaDTO);
            //return CreatedAtAction(nameof(ObtenerPersona), new { id = personaCreada.IdPersona }, personaCreada);
        }

        [HttpGet("personas/{id}")]
        public async Task<IActionResult> ObtenerPersona(Guid id)
        {
            _logger.LogInformation($"Obteniendo persona con ID: {id}");

            var persona = await _personasService.ObtenerPersona(id);

            if (persona == null)
            {
                return NotFound();
            }
            PersonasDTO personaDTO = new(persona);

            return Ok(personaDTO);
        }

        [HttpPut("personas/{id}")]
        public async Task<IActionResult> ActualizarPersona(Guid id)//, [FromBody] Personas personaActualizada)
        {
            _logger.LogInformation($"Actualizando persona con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var personaActualizada = JsonSerializer.Deserialize<Personas>(requestBody);

            var persona = await _personasService.ActualizarPersona(id, personaActualizada);

            if (persona == null)
            {
                return NotFound();
            }
            PersonasDTO personaDTO = new(persona);

            return Ok(personaDTO);
        }

        [HttpDelete("personas/{id}")]
        public async Task<IActionResult> EliminarPersona(Guid id)
        {
            _logger.LogInformation($"Eliminando persona con ID: {id}");

            var personaEliminada = await _personasService.EliminarPersona(id);

            if (!personaEliminada)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
