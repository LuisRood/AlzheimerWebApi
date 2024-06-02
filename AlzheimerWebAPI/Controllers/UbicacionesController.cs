using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
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
    public class UbicacionesController : ControllerBase
    {
        private readonly UbicacionesService _ubicacionesService;
        private readonly ILogger<UbicacionesController> _logger;

        public UbicacionesController(UbicacionesService ubicacionesService, ILogger<UbicacionesController> logger)
        {
            _ubicacionesService = ubicacionesService;
            _logger = logger;
        }

        [HttpPost("CrearUbicacion")]
        public async Task<IActionResult> CrearUbicacion()//[FromBody] UbicacionesDTO nuevaUbicacionDTO)
        {
            _logger.LogInformation("Creando una nueva ubicación.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevaUbicacionDTO = JsonSerializer.Deserialize<UbicacionesDTO>(requestBody);

            var nuevaUbicacion = new Ubicaciones
            {
                IdUbicacion = nuevaUbicacionDTO.IdUbicacion,
                Ubicacion = new Point(nuevaUbicacionDTO.Longitud, nuevaUbicacionDTO.Latitud) { SRID = 4326 },
                FechaHora = nuevaUbicacionDTO.FechaHora,
                IdDispositivo = nuevaUbicacionDTO.IdDispositivo
            };

            var ubicacionCreada = await _ubicacionesService.CrearUbicacion(nuevaUbicacion);

            return Ok(ubicacionCreada);
            //return CreatedAtAction(nameof(ObtenerUbicacion), new { id = ubicacionCreada.IdUbicacion }, ubicacionCreada);
        }

        [HttpGet("ubicaciones/{id}")]
        public async Task<IActionResult> ObtenerUbicacion(Guid id)
        {
            _logger.LogInformation($"Obteniendo ubicación con ID: {id}");

            var ubicacion = await _ubicacionesService.ObtenerUbicacion(id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            var ubicacionDTO = new UbicacionesDTO(ubicacion);
            return Ok(ubicacionDTO);
        }

        [HttpGet("ubicacionesd/{id}")]
        public async Task<IActionResult> ObtenerUbicacionDispositivo(string id)
        {
            _logger.LogInformation($"Obteniendo ubicación con ID dispositivo: {id}");

            var ubicacion = await _ubicacionesService.ObtenerUbicacionPorDispositivo(id);

            if (ubicacion == null)
            {
                return NotFound();
            }

            var ubicacionDTO = new UbicacionesDTO(ubicacion);
            return Ok(ubicacionDTO);
        }

        [HttpPut("ubicaciones/{id}")]
        public async Task<IActionResult> ActualizarUbicacion(Guid id)//, [FromBody] UbicacionesDTO ubicacionActualizadaDTO)
        {
            _logger.LogInformation($"Actualizando ubicación con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var ubicacionActualizadaDTO = JsonSerializer.Deserialize<UbicacionesDTO>(requestBody);

            var ubicacionActualizada = new Ubicaciones
            {
                Ubicacion = new Point(ubicacionActualizadaDTO.Longitud, ubicacionActualizadaDTO.Latitud) { SRID = 4326 },
                FechaHora = ubicacionActualizadaDTO.FechaHora,
                IdDispositivo = ubicacionActualizadaDTO.IdDispositivo
            };

            var ubicacion = await _ubicacionesService.ActualizarUbicacion(id, ubicacionActualizada);

            if (ubicacion == null)
            {
                return NotFound();
            }

            var ubicacionDTO = new UbicacionesDTO(ubicacion);
            return Ok(ubicacionDTO);
        }

        [HttpDelete("ubicaciones/{id}")]
        public async Task<IActionResult> EliminarUbicacion(Guid id)
        {
            _logger.LogInformation($"Eliminando ubicación con ID: {id}");

            var ubicacionEliminada = await _ubicacionesService.EliminarUbicacion(id);

            if (!ubicacionEliminada)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
