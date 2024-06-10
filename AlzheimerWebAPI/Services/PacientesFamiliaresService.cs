using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class PacientesFamiliaresService
    {
        private readonly AlzheimerContext _context;

        public PacientesFamiliaresService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear relación Pacientes-Familiares
        public async Task<PacientesFamiliares> CrearRelacion(PacientesFamiliares relacion)
        {
            _context.PacientesFamiliares.Add(relacion);
            await _context.SaveChangesAsync();
            return relacion;
        }

        // Obtener relación por ID
        public async Task<PacientesFamiliares> ObtenerRelacion(string id)
        {
            return await _context.PacientesFamiliares.FirstOrDefaultAsync(pf => pf.IdPaciente == id);
        }

        // Obtener  Familiares del paciente
        public async Task<List<Familiares>> ObtenerFamiliares(string id)
        {
            /*return await _context.PacientesFamiliares.Where(pf => pf.IdPaciente == id)
                .Include(pf => pf.IdPacienteNavigation)
                .Include(pf => pf.IdFamiliarNavigation)
                .ToListAsync();*/
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

        // Actualizar relación Pacientes-Familiares
        public async Task<PacientesFamiliares> ActualizarRelacion(Guid id, PacientesFamiliares relacionActualizada)
        {
            var relacion = await _context.PacientesFamiliares.FindAsync(id);

            if (relacion == null)
            {
                return null;
            }

            // Actualizar propiedades de la relación
            relacion.IdPaciente = relacionActualizada.IdPaciente;
            relacion.IdFamiliar = relacionActualizada.IdFamiliar;

            await _context.SaveChangesAsync();

            return relacion;
        }

        // Eliminar relación Pacientes-Familiares
        public async Task<bool> EliminarRelacion(Guid id)
        {
            var relacion = await _context.PacientesFamiliares.FindAsync(id);

            if (relacion == null)
            {
                return false;
            }

            _context.PacientesFamiliares.Remove(relacion);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
