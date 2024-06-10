using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class PacientesCuidadores
{
    public Guid IdCuidaPaciente { get; set; }

    public string IdPaciente { get; set; } = null!;

    public Guid IdCuidador { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    public virtual Dias Dia { get; set; } = null!;

    [JsonIgnore]
    public virtual Cuidadores IdCuidadorNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual Pacientes IdPacienteNavigation { get; set; } = null!;
    public PacientesCuidadores() { }

    public PacientesCuidadores(PacientesCuidadoresDTO pacientesCuidadoresDTO)
    {
        IdCuidaPaciente = pacientesCuidadoresDTO.IdCuidaPaciente;
        IdPaciente = pacientesCuidadoresDTO.IdPaciente;
        IdCuidador = pacientesCuidadoresDTO.IdCuidador;
        HoraInicio = pacientesCuidadoresDTO.HoraInicio;
        HoraFin = pacientesCuidadoresDTO.HoraFin;
        Dia = pacientesCuidadoresDTO.Dia;
    }
}
