using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public class DispositivosDTO//(Dispositivo dispositivos)
{
    public string? IdDispositivo { get; set; } 

    public Guid? IdGeocerca { get; set; }

    public virtual GeocercasDTO? IdGeocercaNavigation { get; set; }

    public virtual ICollection<UbicacionesDTO>? Ubicaciones { get; set; }


    public DispositivosDTO()
    {
    }
    public DispositivosDTO(Dispositivos dispositivos)
    {
        IdDispositivo = dispositivos.IdDispositivo;
        IdGeocerca = dispositivos.IdGeocerca;
        if (dispositivos.IdGeocercaNavigation != null)
        {
            IdGeocercaNavigation = new GeocercasDTO(dispositivos.IdGeocercaNavigation);
        }
        Ubicaciones = dispositivos.Ubicaciones
        .Select(u => new UbicacionesDTO(u)).ToList();
    }
}
