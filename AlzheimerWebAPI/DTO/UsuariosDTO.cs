using AlzheimerWebAPI.Models;
using System;

namespace AlzheimerWebAPI.DTO
{
    public class UsuariosDTO
    {
        public Guid IdUsuario { get; set; }

        public string Correo { get; set; } = null!;

        public string Contrasenia { get; set; } = null!;

        public bool Estado { get; set; }

        public Guid IdTipoUsuario { get; set; }

        public Guid IdPersona { get; set; }

        public UsuariosDTO() { }

        public UsuariosDTO(Usuarios usuario)
        {
            IdUsuario = usuario.IdUsuario;
            Correo = usuario.Correo;
            Contrasenia = usuario.Contrasenia;
            Estado = usuario.Estado;
            IdTipoUsuario = usuario.IdTipoUsuario;
            IdPersona = usuario.IdPersona;
        }
    }
}
