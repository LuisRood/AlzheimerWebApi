using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class Usuarios
{
    public Guid IdUsuario { get; set; }

    public string Correo { get; set; } = null!;

    public string Contrasenia { get; set; } = null!;

    public bool Estado { get; set; }

    public Guid IdTipoUsuario { get; set; }

    public Guid IdPersona { get; set; }

    [JsonIgnore]
    public virtual Cuidadores? Cuidadore { get; set; }

    [JsonIgnore]
    public virtual Familiares? Familiare { get; set; }

    public virtual Personas IdPersonaNavigation { get; set; } = null!;

    public virtual TiposUsuarios IdTipoUsuarioNavigation { get; set; } = null!;
}
