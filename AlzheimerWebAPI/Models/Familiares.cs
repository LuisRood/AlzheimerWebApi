using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class Familiares
{
    public Guid IdFamiliar { get; set; }

    public Guid IdUsuario { get; set; }

    [JsonIgnore]
    public virtual Cuidadores? Cuidador { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<PacientesFamiliares> PacientesFamiliares { get; set; } = new List<PacientesFamiliares>();

    public Familiares() { }

    public Familiares(FamiliaresDTO familiarDTO)
    {
        IdFamiliar = familiarDTO.IdFamiliar;
        IdUsuario = familiarDTO.IdUsuario;
    }
}
