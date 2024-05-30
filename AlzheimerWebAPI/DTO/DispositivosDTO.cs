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

    public DispositivosDTO()
    {
    }
    public DispositivosDTO(Dispositivos dispositivos)
    {
        IdDispositivo = dispositivos.IdDispositivo;
        IdGeocerca = dispositivos.IdGeocerca;
    }
}
