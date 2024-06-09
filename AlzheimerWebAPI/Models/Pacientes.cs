using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class Pacientes
{
    public string IdPaciente { get; set; } = null!;

    public string? IdDispositivo { get; set; }

    public Guid IdPersona { get; set; }

    public virtual Dispositivos? IdDispositivoNavigation { get; set; }

    public virtual Personas IdPersonaNavigation { get; set; } = null!;

    public virtual ICollection<Medicamentos> Medicamentos { get; set; } = new List<Medicamentos>();

    [JsonIgnore]
    public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>();

    public virtual ICollection<PacientesCuidadores> PacientesCuidadores { get; set; } = new List<PacientesCuidadores>();

    public virtual ICollection<PacientesFamiliares> PacientesFamiliares { get; set; } = new List<PacientesFamiliares>();
    
    public Pacientes() { }
    public Pacientes(PacientesDTO pacienteDTO)
    {
        IdPaciente = pacienteDTO.IdPaciente;
        IdDispositivo = pacienteDTO.IdDispositivo;
        IdPersona = pacienteDTO.IdPersona;
        IdDispositivoNavigation = pacienteDTO.IdDispositivoNavigation != null ? new Dispositivos(pacienteDTO.IdDispositivoNavigation) : null!;
        IdPersonaNavigation = pacienteDTO.IdPersonaNavigation;
        Medicamentos = pacienteDTO.Medicamentos ?? new List<Medicamentos>();
        Notificaciones = pacienteDTO.Notificaciones?.Select(n => new Notificaciones(n)).ToList() ?? new List<Notificaciones>();
        PacientesCuidadores = pacienteDTO.PacientesCuidadores ?? new List<PacientesCuidadores>();
        PacientesFamiliares = pacienteDTO.PacientesFamiliares ?? new List<PacientesFamiliares>();
    }
}
