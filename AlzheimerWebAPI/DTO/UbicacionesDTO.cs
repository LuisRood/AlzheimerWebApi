using AlzheimerWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.DTO
{
    public class UbicacionesDTO
    {
        public Guid IdUbicacion { get; set; } 
        public double Latitud { get; set; } 

        public double Longitud { get; set; } 

        public DateTime FechaHora { get; set; } 

        public string IdDispositivo { get; set; }
        public UbicacionesDTO() { }
        public UbicacionesDTO(Ubicaciones ubicacion)
        {
            IdUbicacion = ubicacion.IdUbicacion;

            Latitud = ubicacion.Ubicacion.Y;

            Longitud = ubicacion.Ubicacion.X;

            FechaHora = ubicacion.FechaHora;

            IdDispositivo = ubicacion.IdDispositivo;
        }

    }
}
