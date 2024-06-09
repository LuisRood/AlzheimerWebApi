using AlzheimerWebAPI.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Services
{
    public class AutenticacionService
    {
        private readonly string secretKey;
        private readonly Dictionary<string, List<string>> rolePermissions;
        public AutenticacionService(string secretKey)
        {
            this.secretKey = secretKey;
            rolePermissions = new Dictionary<string, List<string>>
            {
                { "Administrador", new List<string> {"usersForms", "carer_form", "user_form", "familiar_form", "patientMgmt", "familyTab", "carerTab", "patientTab", "devMgmt" } },
                { "Familiar", new List<string> { "usersForms", "carer_form", "zoneScr", "patientMgmt", "bluetooth", "setMedAlarm", "devConnAlarm", "fallAlarm", "medicineAlarm", "zoneAlarm", "location", "medMgmt", "carerTab"} },
                { "Cuidador", new List<string> { "setMedAlarm", "devConnAlarm", "fallAlarm", "medicineAlarm", "zoneAlarm", "location", "medMgmt"} }
            };
        }

        public string GenerarJwtToken(Usuarios usuario)
        {
            Console.WriteLine(usuario.Correo);
            Console.WriteLine(usuario.Contrasenia);
            Console.WriteLine(usuario.IdTipoUsuarioNavigation.TipoUsuario);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Obtener los permisos del usuario basado en su rol
            var role = usuario.IdTipoUsuarioNavigation.TipoUsuario;
            var permissions = rolePermissions.ContainsKey(role) ? rolePermissions[role] : new List<string>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim("role",role),
                new Claim("permissions", string.Join(",", permissions)),  // Agregar permisos como claim
                // Agrega más claims según sea necesario
            };

            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            Console.WriteLine($"Token generado: {tokenString}");
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}