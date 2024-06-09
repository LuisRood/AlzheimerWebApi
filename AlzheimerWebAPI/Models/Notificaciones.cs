using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class Notificaciones
{
    public Guid IdNotificacion { get; set; }

    public string Mensaje { get; set; } = null!;

    public DateTime? Fecha { get; set; }

    public TimeSpan Hora { get; set; }

    public string IdPaciente { get; set; } = null!;

    public Guid IdTipoNotificacion { get; set; }

    [JsonIgnore]
    public virtual Pacientes IdPacienteNavigation { get; set; } = null!;

    [JsonIgnore]
    public virtual TiposNotificaciones IdTipoNotificacionNavigation { get; set; } = null!;

    public Notificaciones() { }

    public Notificaciones(NotificacionesDTO notificacionesDTO)
    {
        IdNotificacion = notificacionesDTO.IdNotificacion;
        Mensaje = notificacionesDTO.Mensaje;
        Fecha = notificacionesDTO.Fecha;
        Hora = notificacionesDTO.Hora;
        IdPaciente = notificacionesDTO.IdPaciente;
    }
}
