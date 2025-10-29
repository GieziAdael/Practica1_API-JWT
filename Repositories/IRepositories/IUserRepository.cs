using Practica_API_JWT.Models;
using Practica_API_JWT.Models.Dtos;

namespace Practica_API_JWT.Repositories.IRepositories
{
    public interface IUserRepository
    {
        Task<ICollection<User>> GetUsers();
        Task<User?> GetUser(int id);
        Task<bool> ExistEmail(string email);
        Task<bool> CreateUser(User user);
        Task<bool> PatchUser(int id, string email);
        Task<bool> DeleteUser(User user);

        //JWT
        Task<UserLoginResponseDto> LoginUser(UserLoginDto userLoginDto);
    }
}
