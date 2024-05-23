using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class CuidadoresService
    {
        private readonly AlzheimerContext _context;

        public CuidadoresService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear cuidador
        public async Task<Cuidadores> CrearCuidador(Cuidadores cuidador)
        {
            _context.Cuidadores.Add(cuidador);
            await _context.SaveChangesAsync();
            return cuidador;
        }

        // Obtener cuidador por ID
        public async Task<Cuidadores> ObtenerCuidador(Guid id)
        {
            return await _context.Cuidadores.FindAsync(id);
        }

        //Obtener relacion paciente cuidadores
        public async Task<List<Cuidadores>> ObtenerPacienteCuidadores(string id)
        {
            return await _context.Cuidadores.Join(
                _context.PacientesCuidadores,
                cuidador => cuidador.IdCuidador,
                pacienteCuidador => pacienteCuidador.IdCuidador,
                (cuidador, pacienteCuidador) => new { cuidador, pacienteCuidador }
                )
                .Where(joined => joined.pacienteCuidador.IdPaciente == id)
                .Select(joined => joined.cuidador)
                .ToListAsync();

        }

        // Actualizar cuidador
        public async Task<Cuidadores> ActualizarCuidador(Guid id, Cuidadores cuidadorActualizado)
        {
            var cuidador = await _context.Cuidadores.FindAsync(id);

            if (cuidador == null)
            {
                return null;
            }

            // Actualizar propiedades del cuidador
            cuidador.IdUsuario = cuidadorActualizado.IdUsuario;
            cuidador.IdFamiliar = cuidadorActualizado.IdFamiliar;

            await _context.SaveChangesAsync();

            return cuidador;
        }

        // Eliminar cuidador
        public async Task<bool> EliminarCuidador(Guid id)
        {
            var cuidador = await _context.Cuidadores.FindAsync(id);

            if (cuidador == null)
            {
                return false;
            }

            _context.Cuidadores.Remove(cuidador);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
