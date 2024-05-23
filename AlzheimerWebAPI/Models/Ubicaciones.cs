using System;
using System.Collections.Generic;
using NetTopologySuite.Geometries;

namespace AlzheimerWebAPI.Models;

public partial class Ubicaciones
{
    public Guid IdUbicacion { get; set; }

    public Point Ubicacion { get; set; } = null!;

    public string IdDispositivo { get; set; } = null!;

    public DateTime FechaHora { get; set; }

    public virtual Dispositivos IdDispositivoNavigation { get; set; } = null!;
}
