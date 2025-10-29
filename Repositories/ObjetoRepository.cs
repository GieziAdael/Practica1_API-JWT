using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Practica_API_JWT.Data;
using Practica_API_JWT.Models;
using Practica_API_JWT.Repositories.IRepositories;
using Practica_API_JWT.Services;

namespace Practica_API_JWT.Repositories
{
    public class ObjetoRepository : IObjetoRepository
    {
        private readonly MyAppDbContext _context;

        public ObjetoRepository(MyAppDbContext context)
        {
            _context = context;
        }
        public async Task<ICollection<Objeto>> GetObjetos()
        {
            return await _context.Objetos.ToListAsync();
        }
        public async Task<Objeto?> GetObjeto(int id)
        {
            return await _context.Objetos.FirstOrDefaultAsync(r => r.IdObject == id);
        }
        public async Task<bool> ExistName(string name)
        {
            if (string.IsNullOrEmpty(name)) return false;
            var nameNormalized = EmailNormalized.NormalizarEmail(name);
            //Si existe retornara true, si no, false
            return await _context.Objetos.AnyAsync(r => r.Name == nameNormalized);
        }
        public async Task<bool> CreateObjeto(Objeto objeto)
        {
            if (objeto == null) return false;
            //Sube a la Db el nombre normalizado (esto para evitar repeticion de objetos)
            var nameNormalized = EmailNormalized.NormalizarEmail(objeto.Name);
            var registro = new Objeto()
            {
                Name = nameNormalized,
                DateObjectCreated = DateTime.Now
            };
            _context.Objetos.Add(registro);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchObjeto(int id, string name)
        {
            if(id == 0 || string.IsNullOrWhiteSpace(name)) return false;
            var registro = await _context.Objetos.FirstOrDefaultAsync( r => r.IdObject == id);
            if (registro == null) return false;
            registro.Name = name;
            _context.Objetos.Update(registro);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteObjeto(Objeto objeto)
        {
            _context.Objetos.Remove(objeto);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
