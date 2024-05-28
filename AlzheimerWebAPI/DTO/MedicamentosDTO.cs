using AlzheimerWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace AlzheimerWebAPI.DTO;

public partial class MedicamentosDTO
{
    public Guid IdMedicamento { get; set; }

    public string Nombre { get; set; } = null!;

    public double Gramaje { get; set; }

    public string Descripcion { get; set; } = null!;

    public string IdPaciente { get; set; } = null!;

    public MedicamentosDTO() { }
    public MedicamentosDTO(Medicamentos medicamento)
    {
        IdMedicamento = medicamento.IdMedicamento;
        Nombre = medicamento.Nombre;
        Gramaje= medicamento.Gramaje;
        Descripcion = medicamento.Descripcion;
        IdPaciente = medicamento.IdPaciente;
    }
}
