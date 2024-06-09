using AlzheimerWebAPI.Models;

namespace AlzheimerWebAPI.DTO
{
    public class NotificacionesDTO
    {
        public Guid IdNotificacion { get; set; }

        public string Mensaje { get; set; } = null!;

        public DateTime? Fecha { get; set; }

        public TimeSpan Hora { get; set; }

        public string IdPaciente { get; set; } = null!;

        public Guid IdTipoNotificacion { get; set; }


        public NotificacionesDTO() { }
        public NotificacionesDTO(Notificaciones notificaciones)
        {
            IdNotificacion = notificaciones.IdNotificacion;
            Mensaje = notificaciones.Mensaje;
            Fecha = notificaciones.Fecha;
            Hora = notificaciones.Hora;
            IdPaciente = notificaciones.IdPaciente;
            IdTipoNotificacion = notificaciones.IdTipoNotificacion;
        }
    }
}
