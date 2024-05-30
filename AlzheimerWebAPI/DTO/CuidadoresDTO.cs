using AlzheimerWebAPI.Models;

namespace AlzheimerWebAPI.DTO
{
    public class CuidadoresDTO
    {
        public Guid IdCuidador { get; set; }

        public Guid IdUsuario { get; set; }

        public Guid? IdFamiliar { get; set; }

        public CuidadoresDTO() { }

        public CuidadoresDTO(Cuidadores cuidador)
        {
            IdCuidador = cuidador.IdCuidador;
            IdUsuario = cuidador.IdUsuario;
            IdFamiliar = cuidador.IdFamiliar;
        }
    }
}
