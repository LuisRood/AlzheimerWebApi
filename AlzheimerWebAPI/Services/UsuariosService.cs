using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class UsuariosService
    {
        private readonly AlzheimerContext _context;

        public UsuariosService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear usuario
        public async Task<Usuarios> CrearUsuario(Usuarios usuario)
        {
            usuario.Contrasenia = this.encryptPassword(usuario.Contrasenia);
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Cargar las propiedades de navegación después de que el usuario se ha guardado en la base de datos
            await _context.Entry(usuario)
                .Reference(u => u.IdTipoUsuarioNavigation)
                .LoadAsync();

            await _context.Entry(usuario)
                .Reference(u => u.IdPersonaNavigation)
                .LoadAsync();
            return usuario;
        }

        // Obtener usuario por ID
        public async Task<Usuarios> ObtenerUsuario(Guid id)
        {
            return await _context.Usuarios
                .Include(u => u.IdTipoUsuarioNavigation)
                .Include(u => u.IdPersonaNavigation)
                .FirstOrDefaultAsync(u => u.IdUsuario ==id);
        }

        // Obtener usuario por correo
        public async Task<Usuarios> ObtenerUsuarioCorreo(string correo)
        {
            return await _context.Usuarios
                .Include(u => u.IdTipoUsuarioNavigation)
                .Include(u => u.IdPersonaNavigation)
                .FirstOrDefaultAsync(u=> u.Correo == correo);
        }

        // Actualizar usuario
        public async Task<Usuarios> ActualizarUsuario(Guid id, Usuarios usuarioActualizado)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return null;
            }

            usuario.Correo = usuarioActualizado.Correo;
            usuario.Contrasenia = this.encryptPassword(usuarioActualizado.Contrasenia);
            usuario.Estado = usuarioActualizado.Estado;
            usuario.IdTipoUsuario = usuarioActualizado.IdTipoUsuario;
            usuario.IdPersona = usuarioActualizado.IdPersona;

            await _context.SaveChangesAsync();

            await _context.Entry(usuario)
                .Reference(u => u.IdTipoUsuarioNavigation)
                .LoadAsync();

            await _context.Entry(usuario)
                .Reference(u => u.IdPersonaNavigation)
                .LoadAsync();

            return usuario;
        }

        // Eliminar usuario
        public async Task<bool> EliminarUsuario(Guid id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return false;
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return true;
        }

        private string encryptPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        public bool verifyPassword(string password,string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }
    }
}
