using System.ComponentModel.DataAnnotations;

namespace Practica_API_JWT.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        [StringLength(250)]
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public DateTime AccountCreated { get; set; }
        public string? Role { get; set; }
    }
}
