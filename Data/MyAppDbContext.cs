using Microsoft.EntityFrameworkCore;
using Practica_API_JWT.Models;

namespace Practica_API_JWT.Data
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options) :base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Objeto> Objetos { get; set; }
    }
}
