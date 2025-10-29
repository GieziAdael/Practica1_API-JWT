using System.ComponentModel.DataAnnotations;

namespace Practica_API_JWT.Models.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime AccountCreated { get; set; }
        public string? Role { get; set; }
    }
}
