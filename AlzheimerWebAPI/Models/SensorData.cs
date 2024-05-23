using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace AlzheimerWebAPI.Models
{
    public partial class SensorData
    {
        public string? Mac { get; set; }
        public bool? Caida { get; set; }
        public string? Error { get; set; }
        public double? Longitud { get; set; }
        public double? Latitud { get; set; }
        public DateTime? Fecha { get; set; }
        public TimeSpan? Hora { get; set; }
    }
}
