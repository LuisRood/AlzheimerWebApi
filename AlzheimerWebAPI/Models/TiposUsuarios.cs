using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class TiposUsuarios
{
    public Guid IdTipoUsuario { get; set; }

    public string? TipoUsuario { get; set; }

    [JsonIgnore]
    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
