using Practica_API_JWT.Models;

namespace Practica_API_JWT.Repositories.IRepositories
{
    public interface IObjetoRepository
    {
        Task<ICollection<Objeto>> GetObjetos();
        Task<Objeto?> GetObjeto(int id);
        Task<bool> ExistName(string name);
        Task<bool> CreateObjeto(Objeto objeto);
        Task<bool> PatchObjeto(int id, string name);
        Task<bool> DeleteObjeto(Objeto objeto);
    }
}
