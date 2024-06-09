using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class PacientesService
    {
        private readonly AlzheimerContext _context;

        public PacientesService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear paciente
        public async Task<Pacientes> CrearPaciente(Pacientes paciente)
        {
            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();
            await _context.Entry(paciente)
                .Reference(p => p.IdDispositivoNavigation)
                .LoadAsync();

            await _context.Entry(paciente)
                .Reference(p => p.IdPersonaNavigation)
                .LoadAsync();
            return paciente;
        }

        // Obtener paciente por ID
        public async Task<Pacientes> ObtenerPaciente(Guid id)
        {
            return await _context.Pacientes
                .Join(_context.PacientesFamiliares,
                    pacientes => pacientes.IdPaciente,
                    pacientesFamiliares => pacientesFamiliares.IdPaciente,
                    (pacientes, pacientesFamiliares) => new { Pacientes = pacientes, PacientesFamiliares = pacientesFamiliares })
                .Join(_context.Familiares,
                    combined => combined.PacientesFamiliares.IdFamiliar,
                    familiares => familiares.IdFamiliar,
                    (combined, familiares) => new { Pacientes = combined.Pacientes, Familiares = familiares })
                .GroupJoin(_context.PacientesCuidadores,
                    combined => combined.Pacientes.IdPaciente,
                    pacientesCuidadores => pacientesCuidadores.IdPaciente,
                    (combined, pacientesCuidadores) => new { Pacientes = combined.Pacientes, Familiares = combined.Familiares, PacientesCuidadores = pacientesCuidadores })
                .SelectMany(
                    combined => combined.PacientesCuidadores.DefaultIfEmpty(),
                    (combined, pacienteCuidador) => new { Pacientes = combined.Pacientes, Familiares = combined.Familiares, PacientesCuidador = pacienteCuidador })
                .Where(combined => combined.Familiares.IdUsuario == id || combined.PacientesCuidador.IdCuidador == id)
                .Select(combined => combined.Pacientes)
                .FirstOrDefaultAsync();
            /*return await _context.Pacientes
                .Join(_context.PacientesFamiliares,
                    pacientes => pacientes.IdPaciente,
                    pacientesFamiliares => pacientesFamiliares.IdPaciente,
                    (pacientes, pacientesFamiliares) => new { Pacientes = pacientes, PacientesFamiliares = pacientesFamiliares })
                .Join(_context.Familiares,
                    combined => combined.PacientesFamiliares.IdFamiliar,
                    familiares => familiares.IdFamiliar,
                    (combined, familiares) => new { Pacientes = combined.Pacientes, Familiares = familiares })
                .Where(combined => combined.Familiares.IdUsuario==id)
                .Select(combined => combined.Pacientes)
                .FirstOrDefaultAsync();*/
        }
        // Obtener pacientes por ID usuario
        public async Task<List<Pacientes>> ObtenerPacientes(Guid id)
        {
            /*var pacientesFamiliaresQuery = _context.Pacientes
                .Join(_context.PacientesFamiliares,
                    pacientes => pacientes.IdPaciente,
                    pacientesFamiliares => pacientesFamiliares.IdPaciente,
                    (pacientes, pacientesFamiliares) => new { Pacientes = pacientes, PacientesFamiliares = pacientesFamiliares })
                .Join(_context.Familiares,
                    combined => combined.PacientesFamiliares.IdFamiliar,
                    familiares => familiares.IdFamiliar,
                    (combined, familiares) => new { Pacientes = combined.Pacientes, Familiares = familiares })
                .Where(combined => combined.Familiares.IdUsuario == id)
                .Select(combined => combined.Pacientes);

            var pacientesCuidadoresQuery = _context.Pacientes
                .GroupJoin(_context.PacientesCuidadores,
                    pacientes => pacientes.IdPaciente,
                    pacientesCuidadores => pacientesCuidadores.IdPaciente,
                    (pacientes, pacientesCuidadores) => new { Pacientes = pacientes, PacientesCuidadores = pacientesCuidadores })
                .SelectMany(
                    combined => combined.PacientesCuidadores.DefaultIfEmpty(),
                    (combined, pacienteCuidador) => new { Pacientes = combined.Pacientes, PacientesCuidador = pacienteCuidador })
                .Where(combined => combined.PacientesCuidador.IdCuidador == id)
                .Select(combined => combined.Pacientes);

            var pacientesUnidos = pacientesFamiliaresQuery
                .Union(pacientesCuidadoresQuery)
                .Distinct();

            return await pacientesUnidos.ToListAsync();*/

            // Primer Query (Pacientes y Cuidadores)
            var query1 = from p in _context.Pacientes
                         join pc in _context.PacientesCuidadores on p.IdPaciente equals pc.IdPaciente
                         join c in _context.Cuidadores on pc.IdCuidador equals c.IdCuidador
                         where c.IdUsuario == id
                         select p;

            // Segundo Query (Pacientes y Familiares)
            var query2 = from p in _context.Pacientes
                         join pf in _context.PacientesFamiliares on p.IdPaciente equals pf.IdPaciente
                         join f in _context.Familiares on pf.IdFamiliar equals f.IdFamiliar
                         where f.IdUsuario == id
                         select p;

            var result1 = query1.ToList();
            var result2 = query2.ToList();

            // Combinar resultados según las condiciones especificadas
            var combinedResults = new List<Pacientes>();

            if (result1.Any() && result2.Any())
            {
                combinedResults = result1.Union(result2).ToList();
            }
            else if (result1.Any())
            {
                combinedResults = result1;
            }
            else if (result2.Any())
            {
                combinedResults = result2;
            }
            else
            {
                combinedResults = result1.Union(result2).ToList();
            }
            return combinedResults;
        }
        
        //Recuperar todos los pacientes
        public async Task<List<Pacientes>> ObtenerTodosPacientes()
        {
            return await _context.Pacientes.ToListAsync();
        }

        // Actualizar paciente
        public async Task<Pacientes> ActualizarPaciente(string id, Pacientes pacienteActualizado)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return null;
            }

            // Actualizar propiedades del paciente
            paciente.IdDispositivo = pacienteActualizado.IdDispositivo;
            paciente.IdPersona = pacienteActualizado.IdPersona;

            await _context.SaveChangesAsync();

            return paciente;
        }

        // Eliminar paciente
        public async Task<bool> EliminarPaciente(string id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return false;
            }

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
