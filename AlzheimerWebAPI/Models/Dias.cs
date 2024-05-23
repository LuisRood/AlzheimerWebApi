using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class Dias
{
    public Guid IdDia { get; set; }

    public Guid IdCuidaPaciente { get; set; }

    public bool? Lunes { get; set; }

    public bool? Martes { get; set; }

    public bool? Miercoles { get; set; }

    public bool? Jueves { get; set; }

    public bool? Viernes { get; set; }

    public bool? Sabado { get; set; }

    public bool? Domingo { get; set; }

    [JsonIgnore]
    public virtual PacientesCuidadores IdCuidaPacienteNavigation { get; set; } = null!;
}
