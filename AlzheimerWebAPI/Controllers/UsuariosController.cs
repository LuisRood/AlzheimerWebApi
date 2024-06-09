using AlzheimerWebAPI.DTO;
using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.Notifications;
using AlzheimerWebAPI.Repositories;
using AlzheimerWebAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IHubContext<AlzheimerHub> _hubContext;
        private readonly UsuariosService _usuariosService;
        private readonly PersonasService _personasService;
        private readonly TiposUsuariosService _tiposUsuariosService;
        private readonly FamiliaresService _familiaresService;
        private readonly CuidadoresService _cuidadoresService;
        private readonly DispositivosService _dispositivosService;
        private readonly ILogger<UsuariosController> _logger;
        private readonly AutenticacionService _autenticacionService = new AutenticacionService("770A8A65DA156D24EE2A093277530142");

        public UsuariosController(UsuariosService usuariosService, PersonasService personasService, TiposUsuariosService tiposUsuariosService,
            FamiliaresService familiaresService, CuidadoresService cuidadoresService,DispositivosService dispositivosService,
            IHubContext<AlzheimerHub> hubContext,ILogger<UsuariosController> logger)
        {
            _usuariosService = usuariosService;
            _personasService = personasService;
            _tiposUsuariosService = tiposUsuariosService;
            _familiaresService = familiaresService;
            _cuidadoresService = cuidadoresService;
            _dispositivosService = dispositivosService;
            _hubContext = hubContext;
            _logger = logger;
        }

        [HttpPost("CrearUsuario")]
        public async Task<IActionResult> CrearUsuario()//[FromBody] Users nuevoUser)
        {
            _logger.LogInformation("Creando un nuevo usuario.");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var nuevoUser = JsonSerializer.Deserialize<Users>(requestBody);

            Personas persona = await _personasService.CrearPersona(nuevoUser.Persona);
            Usuarios usuarioCreado = new Usuarios();
            TiposUsuarios tipoUsuario = await _tiposUsuariosService.ObtenerTipoUsuario(nuevoUser.Usuario.IdTipoUsuario);
            if (tipoUsuario.TipoUsuario == "Cuidador")
            {
                Cuidadores cuidador = new Cuidadores();
                nuevoUser.Usuario.IdPersona = persona.IdPersona;
                usuarioCreado = await _usuariosService.CrearUsuario(nuevoUser.Usuario);
                cuidador.IdUsuario = usuarioCreado.IdUsuario;
                cuidador = await _cuidadoresService.CrearCuidador(cuidador);
            }
            else if (tipoUsuario.TipoUsuario == "Familiar")
            {
                Familiares familiar = new Familiares();
                nuevoUser.Usuario.IdPersona = persona.IdPersona;
                usuarioCreado = await _usuariosService.CrearUsuario(nuevoUser.Usuario);
                familiar.IdUsuario = usuarioCreado.IdUsuario;
                familiar = await _familiaresService.CrearFamiliar(familiar);
            }
            else
            {
                nuevoUser.Usuario.IdPersona = persona.IdPersona;
                usuarioCreado = await _usuariosService.CrearUsuario(nuevoUser.Usuario);
            }

            return Ok(usuarioCreado);
            //return CreatedAtAction(nameof(ObtenerUsuario), new { id = usuarioCreado.IdUsuario }, usuarioCreado);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginUsuario([FromBody] LogIn data)
        {
            _logger.LogInformation("Iniciando sesión");
            string correo = data.Correo;
            string contrasenia = data.Contrasenia;

            Usuarios usuarioBd = await _usuariosService.ObtenerUsuarioCorreo(correo);

            if (usuarioBd == null)
            {
                return NotFound();
            }
            if (_usuariosService.verifyPassword(contrasenia, usuarioBd.Contrasenia))
            {
                string token = _autenticacionService.GenerarJwtToken(usuarioBd);

                List<string?>? dispositivos = await _dispositivosService.ObtenerDispositivosPorCorreoUsuario(correo);

                return Ok(new { token,dispositivos });
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpGet("usuarios/{id}")]
        public async Task<IActionResult> ObtenerUsuario(Guid id)
        {
            _logger.LogInformation($"Obteniendo usuario con ID: {id}");

            var usuario = await _usuariosService.ObtenerUsuario(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPut("usuarios/{id}")]
        public async Task<IActionResult> ActualizarUsuario(Guid id)//, [FromBody] Users actualizarUser)
        {
            _logger.LogInformation($"Actualizando usuario con ID: {id}");

            using var reader = new StreamReader(HttpContext.Request.Body);
            var requestBody = await reader.ReadToEndAsync();
            var actualizarUser = JsonSerializer.Deserialize<Users>(requestBody);

            Personas personas = await _personasService.ActualizarPersona(actualizarUser.Persona.IdPersona, actualizarUser.Persona);
            var usuarioActualizado = await _usuariosService.ActualizarUsuario(actualizarUser.Usuario.IdUsuario, actualizarUser.Usuario);

            if (usuarioActualizado == null)
            {
                return NotFound();
            }

            return Ok(usuarioActualizado);
        }

        [HttpDelete("usuarios/{id}")]
        public async Task<IActionResult> EliminarUsuario(Guid id)
        {
            _logger.LogInformation($"Eliminando usuario con ID: {id}");

            var usuarioEliminado = await _usuariosService.EliminarUsuario(id);

            if (!usuarioEliminado)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
