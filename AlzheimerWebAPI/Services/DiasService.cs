using AlzheimerWebAPI.Models;
using AlzheimerWebAPI.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace AlzheimerWebAPI.Repositories
{
    public class DiasService
    {
        private readonly AlzheimerContext _context;

        public DiasService(AlzheimerContext context)
        {
            _context = context;
        }

        public async Task<DiasDTO> CrearDia(Dias dia)
        {
            _context.Dias.Add(dia);
            await _context.SaveChangesAsync();
            return new DiasDTO(dia);
        }

        public async Task<DiasDTO> ObtenerDia(Guid id)
        {
            var dia = await _context.Dias.FindAsync(id);
            if (dia == null)
            {
                return null;
            }
            return new DiasDTO(dia);
        }

        public async Task<DiasDTO> ActualizarDia(Guid id, Dias diaActualizado)
        {
            var dia = await _context.Dias.FindAsync(id);
            if (dia == null)
            {
                return null;
            }

            dia.Lunes = diaActualizado.Lunes;
            dia.Martes = diaActualizado.Martes;
            dia.Miercoles = diaActualizado.Miercoles;
            dia.Jueves = diaActualizado.Jueves;
            dia.Viernes = diaActualizado.Viernes;
            dia.Sabado = diaActualizado.Sabado;
            dia.Domingo = diaActualizado.Domingo;

            await _context.SaveChangesAsync();
            return new DiasDTO(dia);
        }

        public async Task<bool> EliminarDia(Guid id)
        {
            var dia = await _context.Dias.FindAsync(id);
            if (dia == null)
            {
                return false;
            }

            _context.Dias.Remove(dia);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
