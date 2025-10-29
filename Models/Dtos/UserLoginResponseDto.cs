namespace Practica_API_JWT.Models.Dtos
{
    public class UserLoginResponseDto
    {
        public UserRegisterDto? User {  get; set; }
        public string? Token {  get; set; }
        public string? Mensaje { get; set; }
    }
}
