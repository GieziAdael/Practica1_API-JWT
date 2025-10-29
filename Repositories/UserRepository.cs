using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Practica_API_JWT.Data;
using Practica_API_JWT.Models;
using Practica_API_JWT.Models.Dtos;
using Practica_API_JWT.Repositories.IRepositories;
using Practica_API_JWT.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Practica_API_JWT.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly MyAppDbContext _context;
        private readonly string? secretKey;

        public UserRepository(MyAppDbContext context, IConfiguration configuration)
        {
            _context = context;

            //JWT -> Program.cs -> appsettings.json
            secretKey = configuration.GetValue<string>("ApiSettings:SecretKey");
        }
        public async Task<ICollection<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<User?> GetUser(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<bool> ExistEmail(string email)
        {
            if(string.IsNullOrEmpty(email)) return false;
            var emailNormalized = EmailNormalized.NormalizarEmail(email);
            //Si existe retornara true, si no, false
            return await _context.Users.AnyAsync(r => r.Email == emailNormalized);
        }
        public async Task<bool> CreateUser(User user)
        {
            //Validaciones
            if(user == null) return false;
            if(user.Role != "admin" && user.Role != "modd" && user.Role != "viewer")
            {
                return false;
            }

            //Sube a la Db el email normalizado y el password hasheado
            var emailNormalized = EmailNormalized.NormalizarEmail(user.Email);
            var passHash = PasswordHash.Hash(user.PasswordHash);
            var registro = new User()
            {
                Email = emailNormalized,
                PasswordHash = passHash,
                AccountCreated = DateTime.Now,
                Role = user.Role,
            };
            _context.Users.Add(registro);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> PatchUser(int id, string email)
        {
            if(string.IsNullOrEmpty(email) || id <=0)
                return false;
            var registro = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if(registro == null) return false;
            registro.Email = email;
            _context.Users.Update(registro);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUser(User user)
        {
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }


        //JWT
        public async Task<UserLoginResponseDto> LoginUser(UserLoginDto userLoginDto)
        {
            //Comprobaciones
            if (string.IsNullOrEmpty(userLoginDto.Email))
            {
                return new UserLoginResponseDto()
                {
                    User = null,
                    Token = "",
                    Mensaje = "El usuario es requerido"
                };
            }

            var emailNormalized = EmailNormalized.NormalizarEmail(userLoginDto.Email);
            var registro = await _context.Users.FirstOrDefaultAsync(r => r.Email == emailNormalized);
            if (registro is null)
            {
                return new UserLoginResponseDto()
                {
                    User = null,
                    Token = "",
                    Mensaje = "No se encontro el usuario"
                };
            }

            bool passVerify = PasswordHash.Verify(userLoginDto.Password, registro.PasswordHash);

            if (!passVerify)
            {
                return new UserLoginResponseDto()
                {
                    User = null,
                    Token = "",
                    Mensaje = "Credenciales incorrectas"
                };
            }

            //Generacion Token
            //1 encargado de construir y firmar el token
            var handlerToken = new JwtSecurityTokenHandler(); 
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new InvalidOperationException("Secret key no configurada");
            }
            //2 convertir la llave en bytes
            var key = Encoding.UTF8.GetBytes(secretKey);
            //3 diseñar el Token (id, propiedad, rol de autorizacion) + expiracion + Firma digital
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("id", registro.Id.ToString()),
                    new Claim("email", registro.Email),
                    new Claim(ClaimTypes.Role, registro.Role ?? string.Empty)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //4 Creacion Token
            var token = handlerToken.CreateToken(tokenDescriptor);

            return new UserLoginResponseDto()
            {
                User = new UserRegisterDto()
                {
                    Email = registro.Email,
                    PasswordHash = registro.PasswordHash,
                    AccountCreated = registro.AccountCreated,
                    Role = registro.Role ?? "",
                },
                Token = handlerToken.WriteToken(token),
                Mensaje = "Usuario Logeado Correctamente"
            };

        }
    }
}
