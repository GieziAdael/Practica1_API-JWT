using System.ComponentModel.DataAnnotations;

namespace Practica_API_JWT.Models.Dtos
{
    public class ObjetoDto
    {
        public int IdObject { get; set; }
        public required string Name { get; set; }
        public DateTime DateObjectCreated { get; set; }
    }
}
