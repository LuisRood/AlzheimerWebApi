using System;
using System.Collections.Generic;

namespace AlzheimerWebAPI.Models;

public partial class Pacientes
{
    public string IdPaciente { get; set; } = null!;

    public string? IdDispositivo { get; set; }

    public Guid IdPersona { get; set; }

    public virtual Dispositivos? IdDispositivoNavigation { get; set; }

    public virtual Personas IdPersonaNavigation { get; set; } = null!;

    public virtual ICollection<Medicamentos> Medicamentos { get; set; } = new List<Medicamentos>();

    public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>();

    public virtual ICollection<PacientesCuidadores> PacientesCuidadores { get; set; } = new List<PacientesCuidadores>();

    public virtual ICollection<PacientesFamiliares> PacientesFamiliares { get; set; } = new List<PacientesFamiliares>();
}
