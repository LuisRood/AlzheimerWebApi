using AlzheimerWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlzheimerWebAPI.Repositories
{
    public class PersonasService
    {
        private readonly AlzheimerContext _context;

        public PersonasService(AlzheimerContext alzheimerContext)
        {
            _context = alzheimerContext;
        }
        public async Task<Personas> CrearPersona(Personas persona)
        {
            _context.Personas.Add(persona);
            await _context.SaveChangesAsync();
            return persona;
        }

        public async Task<Personas> ObtenerPersona(Guid id)
        {
            return await _context.Personas.FindAsync(id);
        }

        public async Task<Personas> ActualizarPersona(Guid id, Personas personaActualizada)
        {
            var persona = await _context.Personas.FindAsync(id);
            Console.WriteLine("Entro en Personas :" + id);
            if (persona == null)
            {
                return null;
            }

            persona.Nombre = personaActualizada.Nombre;
            persona.ApellidoP = personaActualizada.ApellidoP;
            persona.ApellidoM = personaActualizada.ApellidoM;
            persona.FechaNacimiento = personaActualizada.FechaNacimiento;
            persona.NumeroTelefono = personaActualizada.NumeroTelefono;

            await _context.SaveChangesAsync();

            return persona;
        }

        public async Task<bool> EliminarPersona(Guid id)
        {
            var persona = await _context.Personas.FindAsync(id);

            if (persona == null)
            {
                return false;
            }

            _context.Personas.Remove(persona);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
