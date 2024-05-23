using AlzheimerWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.DTO
{
    internal class PacientesFamiliaresDTO
    {
        public Guid IdPacienteFamiliar { get; set; }

        public string IdPaciente { get; set; } = null!;

        public Guid IdFamiliar { get; set; }

        public virtual Familiares IdFamiliarNavigation { get; set; } = null!;

        public virtual PacientesDTO IdPacienteNavigation { get; set; } = null!;

        public PacientesFamiliaresDTO(PacientesFamiliares pacientesFamiliares)
        {
            IdPacienteFamiliar = pacientesFamiliares.IdPacienteFamiliar;
            IdPaciente = pacientesFamiliares.IdPaciente;
            IdFamiliar = pacientesFamiliares.IdFamiliar;
            IdFamiliarNavigation = pacientesFamiliares.IdFamiliarNavigation;
            IdPacienteNavigation = new PacientesDTO(pacientesFamiliares.IdPacienteNavigation);
        }
    }
}
