using AlzheimerWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class GeocercasService
    {
        private readonly AlzheimerContext _context;

        public GeocercasService(AlzheimerContext context)
        {
            _context = context;
        }

        public async Task<Geocercas> crearGeocerca(Geocercas geocercas)
        {
            _context.Geocercas.Add(geocercas);
            await _context.SaveChangesAsync();
            return geocercas;
        }

        public async Task<Geocercas> ObtenerGeocerca(Guid id)
        {
            return await _context.Geocercas.FindAsync(id);
        }


        public async Task<Geocercas> ActualizarGeocerca(Guid id, Geocercas geocercaActualizada)
        {
            var geocerca = await _context.Geocercas.FindAsync(id);

            if (geocerca == null)
            {
                return null;
            }

            geocerca.CoordenadaInicial = geocercaActualizada.CoordenadaInicial;
            geocerca.RadioGeocerca = geocercaActualizada.RadioGeocerca;
            geocerca.Fecha = geocercaActualizada.Fecha;

            await _context.SaveChangesAsync();

            return geocerca;
        }

        public async Task<bool> EliminarGeocerca(Guid id)
        {
            var geocerca = await _context.Geocercas.FindAsync(id);

            if (geocerca == null)
            {
                return false;
            }

            _context.Geocercas.Remove(geocerca);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
