using System.ComponentModel.DataAnnotations;

namespace Practica_API_JWT.Models
{
    public class Objeto
    {
        [Key]
        public int IdObject { get; set; }
        [StringLength(50)]
        public required string Name { get; set; }
        public DateTime DateObjectCreated { get; set; }
    }
}
