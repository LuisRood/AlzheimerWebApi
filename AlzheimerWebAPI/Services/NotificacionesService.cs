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


        public async Task<List<Notificaciones>> ObtenerNotificacionesParaUsuario(Guid idUsuario)
        {
            var notificacionesCuidadores = await _context.Notificaciones
                .Include(n => n.IdPacienteNavigation)
                .ThenInclude(p => p.PacientesCuidadores)
                .ThenInclude(pc => pc.IdCuidadorNavigation)
                .Where(n => n.IdPacienteNavigation.PacientesCuidadores.Any(pc => pc.IdCuidadorNavigation.IdUsuario == idUsuario))
                .ToListAsync();

            var notificacionesFamiliares = await _context.Notificaciones
                .Include(n => n.IdPacienteNavigation)
                .ThenInclude(p => p.PacientesFamiliares)
                .ThenInclude(pf => pf.IdFamiliarNavigation)
                .Where(n => n.IdPacienteNavigation.PacientesFamiliares.Any(pf => pf.IdFamiliarNavigation.IdUsuario == idUsuario))
                .ToListAsync();

            if (notificacionesCuidadores.Count == 0 && notificacionesFamiliares.Count != 0)
            {
                return notificacionesFamiliares;
            }

            if (notificacionesFamiliares.Count == 0 && notificacionesCuidadores.Count != 0)
            {
                return notificacionesCuidadores;
            }

            if (notificacionesCuidadores.Count != 0 && notificacionesFamiliares.Count != 0)
            {
                return notificacionesCuidadores.Concat(notificacionesFamiliares).ToList();
            }

            return new List<Notificaciones>();
        }
    }
}
