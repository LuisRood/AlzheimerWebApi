using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using NetTopologySuite.Geometries;

namespace AlzheimerWebAPI.Models;

public partial class Geocercas
{
    public Guid IdGeocerca { get; set; }

    public Point CoordenadaInicial { get; set; } = null!;

    public double RadioGeocerca { get; set; }

    public DateTime Fecha { get; set; }

    [JsonIgnore]
    public virtual Dispositivos? Dispositivo { get; set; }
}
