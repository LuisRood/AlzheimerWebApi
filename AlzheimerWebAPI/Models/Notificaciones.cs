using System;
using System.Collections.Generic;

namespace AlzheimerWebAPI.Models;

public partial class Notificaciones
{
    public Guid IdNotificacion { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateOnly? Fecha { get; set; }

    public TimeOnly Hora { get; set; }

    public string IdPaciente { get; set; } = null!;

    public Guid IdTipoNotificacion { get; set; }

    public virtual Pacientes IdPacienteNavigation { get; set; } = null!;

    public virtual TiposNotificaciones IdTipoNotificacionNavigation { get; set; } = null!;
}
