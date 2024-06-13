using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AlzheimerWebAPI.Services
{
    public class NotificacionesService(AlzheimerContext context)
    {
        private readonly AlzheimerContext _context = context;

        public async Task<Notificaciones> CrearNotificacion(Notificaciones notificacion)
        {
            _context.Notificaciones.Add(notificacion);

            await _context.SaveChangesAsync();

            await _context.Entry(notificacion)
                .Reference(n => n.IdTipoNotificacionNavigation)
                .LoadAsync();

            await _context.Entry(notificacion)
                .Reference(n => n.IdPacienteNavigation)
                .LoadAsync();

            return notificacion;
        }
        public async Task<List<Notificaciones>> ObtenerNotificacionesPendientes(TimeSpan hora,TimeSpan minantes)
        {
            return await _context.Notificaciones
            .Where(n => n.Hora >= minantes && n.Hora <= hora && n.Enviada!=null && n.Enviada == false)
            .ToListAsync();
        }
        // Actualizar medicamento
        public async Task<Notificaciones> ActualizarNotificacion(Guid id, Notificaciones notificacionActualizada)
        {
            var notificacion = await _context.Notificaciones.FindAsync(id);

            if (notificacion == null)
            {
                return null;
            }

            // Actualizar propiedades del medicamento
            notificacion.Mensaje = notificacionActualizada.Mensaje;
            notificacion.Fecha = notificacionActualizada.Fecha;
            notificacion.Hora = notificacionActualizada.Hora;
            notificacion.IdPaciente = notificacionActualizada.IdPaciente;
            notificacion.Enviada = notificacionActualizada.Enviada;

            await _context.SaveChangesAsync();

            return notificacion;
        }

        public async Task<List<Notificaciones>> ObtenerNotificacionesParaUsuario(Guid idUsuario)
        {
            var notificacionesCuidadores = await _context.Notificaciones
                .Include(n => n.IdPacienteNavigation)
                .ThenInclude(p => p.PacientesCuidadores)
                .ThenInclude(pc => pc.IdCuidadorNavigation)
                .Where(n => n.IdPacienteNavigation.PacientesCuidadores.Any(pc => pc.IdCuidadorNavigation.IdUsuario == idUsuario)
                && n.IdTipoNotificacion != new Guid("E7C6F965-66D1-48C5-B38A-3B5EBC1966B1"))
                .ToListAsync();

            var notificacionesFamiliares = await _context.Notificaciones
                .Include(n => n.IdPacienteNavigation)
                .ThenInclude(p => p.PacientesFamiliares)
                .ThenInclude(pf => pf.IdFamiliarNavigation)
                .Where(n => n.IdPacienteNavigation.PacientesFamiliares.Any(pf => pf.IdFamiliarNavigation.IdUsuario == idUsuario)
                && n.IdTipoNotificacion != new Guid("E7C6F965-66D1-48C5-B38A-3B5EBC1966B1"))
                .ToListAsync();

            if (notificacionesCuidadores.Count == 0 && notificacionesFamiliares.Count != 0)
            {
                return notificacionesFamiliares
                    .OrderByDescending(n => n.Fecha)
                    .ThenByDescending(n => n.Hora)
                    .ToList();
            }

            if (notificacionesFamiliares.Count == 0 && notificacionesCuidadores.Count != 0)
            {
                return notificacionesCuidadores
                    .OrderByDescending(n => n.Fecha)
                    .ThenByDescending(n => n.Hora)
                    .ToList();
            }

            if (notificacionesCuidadores.Count != 0 && notificacionesFamiliares.Count != 0)
            {
                return notificacionesCuidadores
                    .Concat(notificacionesFamiliares)
                    .OrderByDescending(n => n.Fecha)
                    .ThenByDescending(n => n.Hora)
                    .ToList();
            }

            return new List<Notificaciones>();
        }
        public async Task<List<Notificaciones>> ObtenerNotificacionesParaUsuarioMed(Guid idUsuario)
        {
            var notificacionesCuidadores = await _context.Notificaciones
                .Include(n => n.IdPacienteNavigation)
                .ThenInclude(p => p.PacientesCuidadores)
                .ThenInclude(pc => pc.IdCuidadorNavigation)
                .Where(n => n.IdPacienteNavigation.PacientesCuidadores.Any(pc => pc.IdCuidadorNavigation.IdUsuario == idUsuario)
                &&n.IdTipoNotificacion == new Guid("E7C6F965-66D1-48C5-B38A-3B5EBC1966B1"))
                .ToListAsync();

            var notificacionesFamiliares = await _context.Notificaciones
                .Include(n => n.IdPacienteNavigation)
                .ThenInclude(p => p.PacientesFamiliares)
                .ThenInclude(pf => pf.IdFamiliarNavigation)
                .Where(n => n.IdPacienteNavigation.PacientesFamiliares.Any(pf => pf.IdFamiliarNavigation.IdUsuario == idUsuario)
                && n.IdTipoNotificacion == new Guid("E7C6F965-66D1-48C5-B38A-3B5EBC1966B1"))
                .ToListAsync();

            if (notificacionesCuidadores.Count == 0 && notificacionesFamiliares.Count != 0)
            {
                return notificacionesFamiliares
                    .OrderByDescending(n => n.Fecha)
                    .ThenByDescending(n => n.Hora)
                    .ToList();
            }

            if (notificacionesFamiliares.Count == 0 && notificacionesCuidadores.Count != 0)
            {
                return notificacionesCuidadores
                    .OrderByDescending(n => n.Fecha)
                    .ThenByDescending(n => n.Hora)
                    .ToList();
            }

            if (notificacionesCuidadores.Count != 0 && notificacionesFamiliares.Count != 0)
            {
                return notificacionesCuidadores
                    .Concat(notificacionesFamiliares)
                    .OrderByDescending(n => n.Fecha)
                    .ThenByDescending(n => n.Hora)
                    .ToList();
            }

            return new List<Notificaciones>();
        }
    }
}
