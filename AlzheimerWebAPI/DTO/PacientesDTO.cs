using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;

namespace AlzheimerWebAPI.Models;

public partial class PacientesDTO//(Pacientes paciente)
{
    public string IdPaciente { get; set; } 

    public string? IdDispositivo { get; set; } 

    public Guid IdPersona { get; set; }

    public virtual DispositivosDTO? IdDispositivoNavigation { get; set; }

    public virtual Personas IdPersonaNavigation { get; set; } 

    public virtual ICollection<Medicamentos> Medicamentos { get; set; }

    public virtual ICollection<NotificacionesDTO> Notificaciones { get; set; }

    public virtual ICollection<PacientesCuidadores> PacientesCuidadores { get; set; } 

    public virtual ICollection<PacientesFamiliares> PacientesFamiliares { get; set; }

    public PacientesDTO() { }

    public PacientesDTO(Pacientes paciente)
    {
        IdPaciente = paciente.IdPaciente;
        IdDispositivo = paciente.IdDispositivo;
        IdPersona = paciente.IdPersona;
        IdDispositivoNavigation = paciente.IdDispositivoNavigation != null ? new DispositivosDTO(paciente.IdDispositivoNavigation) : null!;
        IdPersonaNavigation = paciente.IdPersonaNavigation;
        Medicamentos = paciente.Medicamentos ?? new List<Medicamentos>();
        Notificaciones = paciente.Notificaciones?.Select(n => new NotificacionesDTO(n)).ToList() ?? new List<NotificacionesDTO>();
        PacientesCuidadores = paciente.PacientesCuidadores ?? new List<PacientesCuidadores>();
        PacientesFamiliares = paciente.PacientesFamiliares ?? new List<PacientesFamiliares>();
    }
}
