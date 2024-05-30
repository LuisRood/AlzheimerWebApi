using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.Models;

public partial class Medicamentos
{
    public Guid IdMedicamento { get; set; }

    public string Nombre { get; set; } = null!;

    public double Gramaje { get; set; }

    public string Descripcion { get; set; } = null!;

    public string IdPaciente { get; set; } = null!;

    [JsonIgnore]
    public virtual Pacientes IdPacienteNavigation { get; set; } = null!;

    public Medicamentos() { }

    public Medicamentos(MedicamentosDTO medicamentoDTO)
    {
        IdMedicamento = medicamentoDTO.IdMedicamento;
        Nombre = medicamentoDTO.Nombre;
        Gramaje = medicamentoDTO.Gramaje;
        Descripcion = medicamentoDTO.Descripcion;
        IdPaciente = medicamentoDTO.IdPaciente;
    }
}
