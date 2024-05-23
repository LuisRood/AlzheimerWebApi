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
        public AutenticacionService(string secretKey) 
        {
            this.secretKey = secretKey;
        }

        public string GenerarJwtToken(Usuarios usuario)
        {
            Console.WriteLine(usuario.Correo);
            Console.WriteLine(usuario.Contrasenia);
            Console.WriteLine(usuario.IdTipoUsuarioNavigation.TipoUsuario);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, usuario.IdUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, usuario.Correo),
                new Claim("role",usuario.IdTipoUsuarioNavigation.TipoUsuario),
                // Agrega más claims según sea necesario
            };

            var token = new JwtSecurityToken(
                issuer: "your-issuer",
                audience: "your-audience",
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
