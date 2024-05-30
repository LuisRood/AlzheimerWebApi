using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public class Dispositivos
{
    public string IdDispositivo { get; set; } = null!;

    public Guid? IdGeocerca { get; set; }

    public virtual Geocercas? IdGeocercaNavigation { get; set; }

    [JsonIgnore]
    public virtual Pacientes? Paciente { get; set; }

    public virtual ICollection<Ubicaciones> Ubicaciones { get; set; } = new List<Ubicaciones>();

    public Dispositivos() { }

    public Dispositivos(DispositivosDTO dispositivoDTO)
    {
        IdDispositivo = dispositivoDTO.IdDispositivo;
        IdGeocerca = dispositivoDTO.IdGeocerca;
    }
}
