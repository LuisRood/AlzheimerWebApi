using AlzheimerWebAPI.Models;

namespace AlzheimerWebAPI.DTO
{
    public class PacientesCuidadoresDTO
    {
        public Guid IdCuidaPaciente { get; set; }

        public string IdPaciente { get; set; } = null!;

        public Guid IdCuidador { get; set; }

        public TimeOnly HoraInicio { get; set; }

        public TimeOnly HoraFin { get; set; }

        public virtual Dias Dia { get; set; } = null!;

        public PacientesCuidadoresDTO() { }

        public PacientesCuidadoresDTO(PacientesCuidadores pacientesCuidadores)
        {
            IdCuidaPaciente = pacientesCuidadores.IdCuidaPaciente;
            IdPaciente = pacientesCuidadores.IdPaciente;
            IdCuidador = pacientesCuidadores.IdCuidador;
            HoraInicio = pacientesCuidadores.HoraInicio;
            HoraFin = pacientesCuidadores.HoraFin;
            Dia = pacientesCuidadores.Dia;
        }
    }
}
