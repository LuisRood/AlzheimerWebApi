using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class PacientesFamiliares
{
    public Guid IdPacienteFamiliar { get; set; }

    public string IdPaciente { get; set; } = null!;

    public Guid IdFamiliar { get; set; }

    [JsonIgnore]
    public virtual Familiares IdFamiliarNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Pacientes IdPacienteNavigation { get; set; } = null!;
    public PacientesFamiliares() { }

    public PacientesFamiliares(PacientesFamiliaresDTO pacientesFamiliaresDTO)
    {
        IdPacienteFamiliar = pacientesFamiliaresDTO.IdPacienteFamiliar;
        IdPaciente = pacientesFamiliaresDTO.IdPaciente;
        IdFamiliar = pacientesFamiliaresDTO.IdFamiliar;
    }
}
