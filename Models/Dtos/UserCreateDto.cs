namespace Practica_API_JWT.Models.Dtos
{
    public class UserCreateDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; }
    }
}
