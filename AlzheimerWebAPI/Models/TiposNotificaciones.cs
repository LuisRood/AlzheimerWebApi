using System;
using System.Collections.Generic;

namespace AlzheimerWebAPI.Models;

public partial class TiposNotificaciones
{
    public Guid IdTipoNotificacion { get; set; }

    public string TipoNotificacion { get; set; } = null!;

    public virtual ICollection<Notificaciones> Notificaciones { get; set; } = new List<Notificaciones>();
}
