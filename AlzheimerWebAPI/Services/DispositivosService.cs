using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class DispositivosService
    {
        private readonly AlzheimerContext _context;

        public DispositivosService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear dispositivo
        public async Task<Dispositivos> CrearDispositivo(Dispositivos dispositivo)
        {
            _context.Dispositivos.Add(dispositivo);
            await _context.SaveChangesAsync();

            await _context.Entry(dispositivo)
                .Reference(d => d.IdGeocercaNavigation)
                .LoadAsync();

            await _context.Entry(dispositivo)
                .Collection(d => d.Ubicaciones)
                .LoadAsync();
            return dispositivo;
        }

        // Obtener dispositivo por ID
        public async Task<Dispositivos> ObtenerDispositivo(string id)
        {
            return await _context.Dispositivos.FindAsync(id);
        }

        public async Task<List<Dispositivos>> ObtenerDispositivos()
        {
            return await _context.Dispositivos.ToListAsync();
        }

        public async Task<List<string?>?> ObtenerDispositivosPorCorreoUsuario(string id)
        {
            return await _context.Pacientes
            .Join(_context.PacientesFamiliares,
                pacientes => pacientes.IdPaciente,
                pacientesFamiliares => pacientesFamiliares.IdPaciente,
                (pacientes, pacientesFamiliares) => new { Pacientes = pacientes, PacientesFamiliares = pacientesFamiliares })
            .Join(_context.Familiares,
                combined => combined.PacientesFamiliares.IdFamiliar,
                familiares => familiares.IdFamiliar,
                (combined, familiares) => new { combined.Pacientes, Familiares = familiares })
            .Where(combined => combined.Familiares.IdUsuarioNavigation.Correo == id)
            .Select(combined => combined.Pacientes.IdDispositivo)
            .Union(
                _context.Pacientes
                    .Join(_context.PacientesCuidadores,
                        pacientes => pacientes.IdPaciente,
                        pacientesCuidadores => pacientesCuidadores.IdPaciente,
                        (pacientes, pacientesCuidadores) => new { Pacientes = pacientes, PacientesCuidadores = pacientesCuidadores })
                    .Join(_context.Cuidadores,
                        combined => combined.PacientesCuidadores.IdCuidador,
                        cuidadores => cuidadores.IdCuidador,
                        (combined, cuidadores) => new { combined.Pacientes, Cuidadores = cuidadores })
                    .Where(combined => combined.Cuidadores.IdUsuarioNavigation.Correo == id)
                    .Select(combined => combined.Pacientes.IdDispositivo)
            )
            .ToListAsync();
        }
        // Actualizar dispositivo
        public async Task<Dispositivos> ActualizarDispositivo(string id, Dispositivos dispositivoActualizado)
        {
            var dispositivo = await _context.Dispositivos.FindAsync(id);

            if (dispositivo == null)
            {
                return null;
            }

            // Actualizar propiedades del dispositivo
            dispositivo.IdGeocerca = dispositivoActualizado.IdGeocerca;

            await _context.SaveChangesAsync();
            await _context.Entry(dispositivo)
                .Reference(d => d.IdGeocercaNavigation)
                .LoadAsync();

            await _context.Entry(dispositivo)
                .Collection(d => d.Ubicaciones)
                .LoadAsync();

            return dispositivo;
        }

        // Eliminar dispositivo
        public async Task<bool> EliminarDispositivo(string id)
        {
            var dispositivo = await _context.Dispositivos.FindAsync(id);

            if (dispositivo == null)
            {
                return false;
            }

            _context.Dispositivos.Remove(dispositivo);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
