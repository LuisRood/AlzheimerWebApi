using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class PacientesCuidadoresService
    {
        private readonly AlzheimerContext _context;

        public PacientesCuidadoresService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear relación Pacientes-Cuidador
        public async Task<PacientesCuidadores> CrearRelacion(PacientesCuidadores relacion)
        {
            _context.PacientesCuidadores.Add(relacion);
            await _context.SaveChangesAsync();
            return relacion;
        }

        // Obtener relación por ID de paciente
        public async Task<PacientesCuidadores> ObtenerRelacion(string id)
        {
            return await _context.PacientesCuidadores.FirstOrDefaultAsync(pc => pc.IdPaciente == id);
        }
        // Actualizar relación Pacientes-Cuidador
        public async Task<PacientesCuidadores> ActualizarRelacion(Guid id, PacientesCuidadores relacionActualizada)
        {
            var relacion = await _context.PacientesCuidadores.FindAsync(id);

            if (relacion == null)
            {
                return null;
            }

            // Actualizar propiedades de la relación
            relacion.IdPaciente = relacionActualizada.IdPaciente;
            relacion.IdCuidador = relacionActualizada.IdCuidador;
            relacion.HoraInicio = relacionActualizada.HoraInicio;
            relacion.HoraFin = relacionActualizada.HoraFin;

            await _context.SaveChangesAsync();

            return relacion;
        }

        // Eliminar relación Pacientes-Cuidador
        public async Task<bool> EliminarRelacion(Guid id)
        {
            var relacion = await _context.PacientesCuidadores.FindAsync(id);

            if (relacion == null)
            {
                return false;
            }

            _context.PacientesCuidadores.Remove(relacion);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
