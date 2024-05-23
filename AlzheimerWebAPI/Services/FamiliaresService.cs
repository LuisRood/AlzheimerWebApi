using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class FamiliaresService
    {
        private readonly AlzheimerContext _context;

        public FamiliaresService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear familiar
        public async Task<Familiares> CrearFamiliar(Familiares familiar)
        {
            _context.Familiares.Add(familiar);
            await _context.SaveChangesAsync();

            await _context.Entry(familiar)
                .Reference(p => p.IdUsuarioNavigation)
                .LoadAsync();

            return familiar;
        }

        // Obtener familiar por ID
        public async Task<Familiares> ObtenerFamiliar(Guid id)
        {
            return await _context.Familiares.FindAsync(id);
        }

        public async Task<List<Familiares>> ObtenerFamiliaresPaciente(string id)
        {
            return await _context.Familiares.Join(
            _context.PacientesFamiliares,
            familiar => familiar.IdFamiliar,
            pacienteFamiliar => pacienteFamiliar.IdFamiliar,
            (familiar, pacienteFamiliar) => new { familiar, pacienteFamiliar }
            )
            .Where(joined => joined.pacienteFamiliar.IdPaciente == id)
            .Select(joined => joined.familiar)
            .ToListAsync();
        }

        // Actualizar familiar
        public async Task<Familiares> ActualizarFamiliar(Guid id, Familiares familiarActualizado)
        {
            var familiar = await _context.Familiares.FindAsync(id);

            if (familiar == null)
            {
                return null;
            }

            // Actualizar propiedades del familiar
            familiar.IdUsuario = familiarActualizado.IdUsuario;

            await _context.SaveChangesAsync();

            return familiar;
        }

        // Eliminar familiar
        public async Task<bool> EliminarFamiliar(Guid id)
        {
            var familiar = await _context.Familiares.FindAsync(id);

            if (familiar == null)
            {
                return false;
            }

            _context.Familiares.Remove(familiar);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
