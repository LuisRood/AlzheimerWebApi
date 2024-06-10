using AlzheimerWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.DTO
{
    public class PacientesFamiliaresDTO
    {
        public Guid IdPacienteFamiliar { get; set; }

        public string IdPaciente { get; set; } = null!;

        public Guid IdFamiliar { get; set; }

        public PacientesFamiliaresDTO() { }

        public PacientesFamiliaresDTO(PacientesFamiliares pacientesFamiliares)
        {
            IdPacienteFamiliar = pacientesFamiliares.IdPacienteFamiliar;
            IdPaciente = pacientesFamiliares.IdPaciente;
            IdFamiliar = pacientesFamiliares.IdFamiliar;
        }
    }
}
