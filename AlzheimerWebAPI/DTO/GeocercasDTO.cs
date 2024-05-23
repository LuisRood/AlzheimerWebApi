using AlzheimerWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.DTO
{
    public class GeocercasDTO//(Geocercas geocerca)
    {
        public Guid IdGeocerca { get; set; }
        public double RadioGeocerca { get; set; }
        public DateTime Fecha { get; set; }
        public double Latitud { get; set; }
        public double Longitud { get; set; }

        // Constructor sin parámetros
        public GeocercasDTO()
        {
        }

        // Constructor que asigna los valores
        public GeocercasDTO(Geocercas geocerca)
        {
            IdGeocerca = geocerca.IdGeocerca;
            RadioGeocerca = geocerca.RadioGeocerca;
            Fecha = geocerca.Fecha;
            Latitud = geocerca.CoordenadaInicial.Y;
            Longitud = geocerca.CoordenadaInicial.X;
        }
    }
}
