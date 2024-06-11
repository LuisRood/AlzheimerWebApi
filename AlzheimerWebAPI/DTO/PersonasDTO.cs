using AlzheimerWebAPI.Models;
using System;

namespace AlzheimerWebAPI.DTO
{
    public class PersonasDTO
    {
        public Guid IdPersona { get; set; }

        public string Nombre { get; set; } = null!;

        public string ApellidoP { get; set; } = null!;

        public string ApellidoM { get; set; } = null!;

        public DateTime FechaNacimiento { get; set; }

        public string? NumeroTelefono { get; set; }

        public PersonasDTO() { }

        public PersonasDTO(Personas persona)
        {
            IdPersona = persona.IdPersona;
            Nombre = persona.Nombre;
            ApellidoP = persona.ApellidoP;
            ApellidoM = persona.ApellidoM;
            FechaNacimiento = persona.FechaNacimiento;
            NumeroTelefono = persona.NumeroTelefono;
        }
    }
}
