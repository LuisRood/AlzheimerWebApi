using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class Personas
{
    public Guid IdPersona { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoP { get; set; } = null!;

    public string ApellidoM { get; set; } = null!;

    public DateTime FechaNacimiento { get; set; }

    public string? NumeroTelefono { get; set; }

    [JsonIgnore]
    public virtual Pacientes? Paciente { get; set; }

    [JsonIgnore]
    public virtual Usuarios? Usuario { get; set; }
}
