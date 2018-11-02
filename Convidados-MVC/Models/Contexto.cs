using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Convidados_MVC.Models
{
    public class Contexto : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {              
            optionsBuilder.UseSqlite("Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), @"App_Data\Casamento.db"));
        }

         public DbSet<Convidado> Convidado { get; set; }
         public DbSet<Usuario> Usuario { get; set; }
    }
}