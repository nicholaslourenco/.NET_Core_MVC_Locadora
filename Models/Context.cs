using Microsoft.EntityFrameworkCore;

namespace Locadora.Models
{
    public class Context : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;DataBase=Locadora;Uid=root;",
                new MySqlServerVersion(new Version(10, 4, 24))
            );
        }

        public DbSet<Filme> Filmes {get; set;}
        public DbSet<Usuario> Usuarios {get; set;}
        public DbSet<Locacao> Locacaos {get; set;}

    }
}