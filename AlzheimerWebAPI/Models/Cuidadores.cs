using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;

namespace AlzheimerWebAPI.Models;

public class Cuidadores
{
    public Guid IdCuidador { get; set; }

    public Guid IdUsuario { get; set; }

    public Guid? IdFamiliar { get; set; }

    public virtual Familiares? IdFamiliarNavigation { get; set; }

    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;

    public virtual ICollection<PacientesCuidadores> PacientesCuidadores { get; set; } = new List<PacientesCuidadores>();

    public Cuidadores() { }

    public Cuidadores(CuidadoresDTO cuidadorDTO)
    {
        IdCuidador = cuidadorDTO.IdCuidador;
        IdUsuario = cuidadorDTO.IdUsuario;
        IdFamiliar = cuidadorDTO.IdFamiliar;
    }
}
