using AlzheimerWebAPI.Models;

namespace AlzheimerWebAPI.DTO
{
    public class FamiliaresDTO
    {
        public Guid IdFamiliar { get; set; }

        public Guid IdUsuario { get; set; }

        public FamiliaresDTO() { }

        public FamiliaresDTO(Familiares familiar)
        {
            IdFamiliar = familiar.IdFamiliar;
            IdUsuario = familiar.IdUsuario;
        }
    }
}
