using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
    [Authorize]
    public class DispositivosController : ControllerBase
    {
        private readonly DispositivosService _dispositivosService;
        private readonly ILogger<DispositivosController> _logger;

        public DispositivosController(DispositivosService dispositivosService, ILogger<DispositivosController> logger)
        {
            _dispositivosService = dispositivosService;
            _logger = logger;
        }

        [HttpPost("CrearDispositivo")]
        public async Task<IActionResult> CrearDispositivo()//[FromBody] DispositivosDTO nuevoDispositivoDTO)
        {
            _logger.LogInformation("Creando un nuevo dispositivo.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevoDispositivoDTO = JsonSerializer.Deserialize<DispositivosDTO>(requestBody);

            var nuevoDispositivo = new Dispositivos(nuevoDispositivoDTO);
            var dispositivoCreado = await _dispositivosService.CrearDispositivo(nuevoDispositivo);

            return Ok(dispositivoCreado);
           // return CreatedAtAction(nameof(ObtenerDispositivo), new { id = dispositivoCreado.IdDispositivo }, dispositivoCreado);
        }

        [HttpGet("dispositivos/{id}")]
        public async Task<IActionResult> ObtenerDispositivo(string id)
        {
            _logger.LogInformation($"Obteniendo dispositivo con ID: {id}");

            var dispositivo = await _dispositivosService.ObtenerDispositivo(id);

            if (dispositivo == null)
            {
                return NotFound();
            }

            return Ok(dispositivo);
        }

        [HttpGet("dispositivos/")]
        public async Task<IActionResult> ObtenerDispositivos()
        {
            _logger.LogInformation($"Obteniendo dispositivos");

            var dispositivos = await _dispositivosService.ObtenerDispositivos();

            if (!dispositivos.Any())
            {
                return NotFound();
            }
            var dispositivosDTO = dispositivos.Select(d =>
            new DispositivosDTO(d)).ToList();

            return Ok(dispositivosDTO);
        }

        [HttpPut("dispositivos/{id}")]
        public async Task<IActionResult> ActualizarDispositivo(string id)//, [FromBody] DispositivosDTO dispositivoActualizadoDTO)
        {
            _logger.LogInformation($"Actualizando dispositivo con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var dispositivoActualizadoDTO = JsonSerializer.Deserialize<DispositivosDTO>(requestBody);


            var dispositivoActualizado = new Dispositivos(dispositivoActualizadoDTO);

            /*var dispositivoActualizado = new Dispositivos
            {
                IdGeocerca = dispositivoActualizadoDTO.IdGeocerca
            };*/

            var dispositivo = await _dispositivosService.ActualizarDispositivo(id, dispositivoActualizado);

            if (dispositivo == null)
            {
                return NotFound();
            }

            return Ok(new DispositivosDTO(dispositivo));
        }

        [HttpDelete("dispositivos/{id}")]
        public async Task<IActionResult> EliminarDispositivo(string id)
        {
            _logger.LogInformation($"Eliminando dispositivo con ID: {id}");

            var dispositivoEliminado = await _dispositivosService.EliminarDispositivo(id);

            if (!dispositivoEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
