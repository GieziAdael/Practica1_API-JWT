using System.ComponentModel.DataAnnotations;

namespace Practica_API_JWT.Models.Dtos
{
    public class UserRegisterDto
    {
        public int ID { get; set; }
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime AccountCreated { get; set; }
        public string? Role { get; set; }
    }
}
