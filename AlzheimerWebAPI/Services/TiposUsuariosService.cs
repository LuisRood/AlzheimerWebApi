using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class TiposUsuariosService
    {
        private readonly AlzheimerContext _context;

        public TiposUsuariosService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear tipo de usuario
        public async Task<TiposUsuarios> CrearTipoUsuario(TiposUsuarios tipoUsuario)
        {
            _context.TiposUsuarios.Add(tipoUsuario);
            await _context.SaveChangesAsync();
            return tipoUsuario;
        }

        // Obtener tipo de usuario por ID
        public async Task<TiposUsuarios> ObtenerTipoUsuario(Guid id)
        {
            return await _context.TiposUsuarios.FindAsync(id);
        }

        public async Task<TiposUsuarios> ObtenerTiposUsuarioTipo(string tipoUsuario)
        {
            return await _context.TiposUsuarios
                .FirstOrDefaultAsync(u => u.TipoUsuario == tipoUsuario);
        }
        // Actualizar tipo de usuario
        public async Task<TiposUsuarios> ActualizarTipoUsuario(Guid id, TiposUsuarios tipoUsuarioActualizado)
        {
            var tipoUsuario = await _context.TiposUsuarios.FindAsync(id);

            if (tipoUsuario == null)
            {
                return null;
            }

            // Actualizar propiedades del tipo de usuario
            tipoUsuario.TipoUsuario = tipoUsuarioActualizado.TipoUsuario;

            await _context.SaveChangesAsync();

            return tipoUsuario;
        }

        // Eliminar tipo de usuario
        public async Task<bool> EliminarTipoUsuario(Guid id)
        {
            var tipoUsuario = await _context.TiposUsuarios.FindAsync(id);

            if (tipoUsuario == null)
            {
                return false;
            }

            _context.TiposUsuarios.Remove(tipoUsuario);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
