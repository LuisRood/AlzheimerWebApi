using AlzheimerWebAPI.Models;
using System;

namespace AlzheimerWebAPI.DTO
{
    public class DiasDTO
    {
        public Guid IdDia { get; set; }

        public Guid IdCuidaPaciente { get; set; }

        public bool? Lunes { get; set; }

        public bool? Martes { get; set; }

        public bool? Miercoles { get; set; }

        public bool? Jueves { get; set; }

        public bool? Viernes { get; set; }

        public bool? Sabado { get; set; }

        public bool? Domingo { get; set; }

        public DiasDTO() { }

        public DiasDTO(Dias dia)
        {
            IdDia = dia.IdDia;
            IdCuidaPaciente = dia.IdCuidaPaciente;
            Lunes = dia.Lunes;
            Martes = dia.Martes;
            Miercoles = dia.Miercoles;
            Jueves = dia.Jueves;
            Viernes = dia.Viernes;
            Sabado = dia.Sabado;
            Domingo = dia.Domingo;
        }
    }
}
