using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Models
{
    public class LogIn
    {
        public string Correo { get; set; } = null!;

        public string Contrasenia { get; set; } = null!;

        public List<string> Dispositivos { get; set; } = [];
    }
}
