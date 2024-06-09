using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace AlzheimerWebAPI.Services
{
    public class TiposNotificacionesService(AlzheimerContext context)
    {
        private readonly AlzheimerContext _context = context;

        public async Task<TiposNotificaciones?> ObtenerNotificacion(Guid id)
        {
            return await _context.TiposNotificaciones.FindAsync(id);
        }
    }
}
