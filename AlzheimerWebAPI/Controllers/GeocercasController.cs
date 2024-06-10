using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    [Authorize]
    public class GeocercasController : ControllerBase
    {
        private readonly GeocercasService _geocercasService;
        private readonly ILogger<GeocercasController> _logger;

        public GeocercasController(GeocercasService geocercasService, ILogger<GeocercasController> logger)
        {
            _geocercasService = geocercasService;
            _logger = logger;
        }

        [HttpPost("CrearGeocerca")]
        public async Task<IActionResult> CrearGeocerca()//[FromBody] GeocercasDTO nuevaGeocercaDTO)
        {
            _logger.LogInformation("Creando nueva Geocerca.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevaGeocercaDTO = JsonSerializer.Deserialize<GeocercasDTO>(requestBody);

            var nuevaGeocerca = new Geocercas
            {
                RadioGeocerca = nuevaGeocercaDTO.RadioGeocerca,
                Fecha = nuevaGeocercaDTO.Fecha,
                CoordenadaInicial = new Point(nuevaGeocercaDTO.Longitud, nuevaGeocercaDTO.Latitud) { SRID = 4326 },
            };

            var geocercaCreada = await _geocercasService.crearGeocerca(nuevaGeocerca);
            var geocercaCreadaDTO = new GeocercasDTO(geocercaCreada);

            //return CreatedAtAction(nameof(ObtenerGeocerca), new { id = geocercaCreadaDTO.IdGeocerca }, geocercaCreadaDTO);
            return Ok(geocercaCreadaDTO);
        }

        [HttpGet("geocercas/{id}")]
        public async Task<IActionResult> ObtenerGeocerca(Guid id)
        {
            _logger.LogInformation($"Obteniendo geocerca con ID: {id}");

            var geocerca = await _geocercasService.ObtenerGeocerca(id);

            if (geocerca == null)
            {
                return NotFound();
            }

            var geocercaDTO = new GeocercasDTO(geocerca);

            return Ok(geocercaDTO);
        }

        [HttpPut("geocercas/{id}")]
        public async Task<IActionResult> ActualizarGeocerca(Guid id)//, [FromBody] GeocercasDTO geocercaActualizadaDTO)
        {
            _logger.LogInformation($"Actualizando geocerca con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var geocercaActualizadaDTO = JsonSerializer.Deserialize<GeocercasDTO>(requestBody);

            var geocercaActualizada = new Geocercas
            {
                RadioGeocerca = geocercaActualizadaDTO.RadioGeocerca,
                Fecha = geocercaActualizadaDTO.Fecha,
                CoordenadaInicial = new Point(geocercaActualizadaDTO.Longitud, geocercaActualizadaDTO.Latitud) { SRID = 4326 },
            };

            var geocerca = await _geocercasService.ActualizarGeocerca(id, geocercaActualizada);

            if (geocerca == null)
            {
                return NotFound();
            }

            var geocercaDTO = new GeocercasDTO(geocerca);

            return Ok(geocercaDTO);
        }

        [HttpDelete("geocercas/{id}")]
        public async Task<IActionResult> EliminarGeocerca(Guid id)
        {
            _logger.LogInformation($"Eliminando geocerca con ID: {id}");

            var geocercaEliminada = await _geocercasService.EliminarGeocerca(id);

            if (!geocercaEliminada)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
