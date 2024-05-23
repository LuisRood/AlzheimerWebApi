using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class MedicamentosService
    {
        private readonly AlzheimerContext _context;

        public MedicamentosService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear medicamento
        public async Task<Medicamentos> CrearMedicamento(Medicamentos medicamento)
        {
            _context.Medicamentos.Add(medicamento);
            await _context.SaveChangesAsync();
            return medicamento;
        }

        // Obtener medicamento por ID
        public async Task<Medicamentos> ObtenerMedicamento(Guid id)
        {
            return await _context.Medicamentos.FindAsync(id);
        }
        // Obtener medicamentos por id Paciente
        public async Task<List<Medicamentos>> ObtenerMedicamentosPaciente(string id)
        {
            return await _context.Medicamentos.Where(m => m.IdPaciente == id)
                .ToListAsync();
        }
        // Actualizar medicamento
        public async Task<Medicamentos> ActualizarMedicamento(Guid id, Medicamentos medicamentoActualizado)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);

            if (medicamento == null)
            {
                return null;
            }

            // Actualizar propiedades del medicamento
            medicamento.Nombre = medicamentoActualizado.Nombre;
            medicamento.Gramaje = medicamentoActualizado.Gramaje;
            medicamento.Descripcion = medicamentoActualizado.Descripcion;
            medicamento.IdPaciente = medicamentoActualizado.IdPaciente;

            await _context.SaveChangesAsync();

            return medicamento;
        }

        // Eliminar medicamento
        public async Task<bool> EliminarMedicamento(Guid id)
        {
            var medicamento = await _context.Medicamentos.FindAsync(id);

            if (medicamento == null)
            {
                return false;
            }

            _context.Medicamentos.Remove(medicamento);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
