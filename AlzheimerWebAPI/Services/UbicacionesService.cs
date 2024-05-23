using AlzheimerWebAPI.Models;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using System;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class UbicacionesService
    {
        private readonly AlzheimerContext _context;

        public UbicacionesService(AlzheimerContext context)
        {
            _context = context;
        }

        // Crear ubicación
        public async Task<Ubicaciones> CrearUbicacion(Ubicaciones ubicacion)
        {
            _context.Ubicaciones.Add(ubicacion);
            await _context.SaveChangesAsync();
            return ubicacion;
        }

        // Obtener ubicación por ID
        public async Task<Ubicaciones> ObtenerUbicacion(Guid id)
        {
            return await _context.Ubicaciones.FindAsync(id);
        }
        public async Task<Ubicaciones> ObtenerUbicacionPorDispositivo(string id)
        {
            return await _context.Ubicaciones.FirstOrDefaultAsync(u => u.IdDispositivo == id);
        }


        // Actualizar ubicación
        public async Task<Ubicaciones> ActualizarUbicacion(Guid id, Ubicaciones ubicacionActualizada)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);

            if (ubicacion == null)
            {
                return null;
            }

            // Actualizar propiedades de la ubicación
            ubicacion.Ubicacion = ubicacionActualizada.Ubicacion;
            ubicacion.FechaHora = ubicacionActualizada.FechaHora;
            ubicacion.IdDispositivo = ubicacionActualizada.IdDispositivo;

            await _context.SaveChangesAsync();

            return ubicacion;
        }
        // Actualizar ubicación por id Dipositivo
        public async Task<Ubicaciones> ActualizarUbicacionDispositivo(string id, Ubicaciones ubicacionActualizada)
        {
            var ubicacion = await _context.Ubicaciones.FirstOrDefaultAsync(d => d.IdDispositivo == id);

            if (ubicacion == null)
            {
                return null;
            }

            // Actualizar propiedades de la ubicación
            ubicacion.Ubicacion = ubicacionActualizada.Ubicacion;
            ubicacion.FechaHora = ubicacionActualizada.FechaHora;
            ubicacion.IdDispositivo = ubicacionActualizada.IdDispositivo;

            await _context.SaveChangesAsync();

            return ubicacion;
        }

        // Eliminar ubicación
        public async Task<bool> EliminarUbicacion(Guid id)
        {
            var ubicacion = await _context.Ubicaciones.FindAsync(id);

            if (ubicacion == null)
            {
                return false;
            }

            _context.Ubicaciones.Remove(ubicacion);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CheckIfOutsideGeofence(string deviceId,Ubicaciones ubicacion)
        {
            var geocerca = await _context.Geocercas
                .Include(g => g.Dispositivo)
                .FirstOrDefaultAsync(g => g.Dispositivo.IdDispositivo == deviceId);

            if(geocerca == null)
            {
                throw new InvalidOperationException("No geofence found for the given device.");
            }
            double metersPerDegree = 111320; // Aproximadamente en el ecuador
            double radiusInDegrees = geocerca.RadioGeocerca / metersPerDegree;
            // Crear un círculo utilizando NetTopologySuite que representa la geocerca
            var circle = new Point(geocerca.CoordenadaInicial.X, geocerca.CoordenadaInicial.Y) { SRID = 4326 }
                            .Buffer(radiusInDegrees);  // Buffer crea un círculo con el radio especificado
                                                              // Verificar si el punto de ubicación está fuera del círculo
                                                              // Si el círculo NO contiene el punto, entonces el dispositivo está fuera de la geocerca
            Console.WriteLine("PuntoInicial: "+geocerca.CoordenadaInicial);
            Console.WriteLine("Radio: "+geocerca.RadioGeocerca);
            bool isOutside = !circle.Contains(ubicacion.Ubicacion);
            return isOutside;
        }
    }
}
