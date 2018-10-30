using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Convidados_MVC.Models
{
    public class Contexto : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {            
            optionsBuilder.UseSqlite("Data Source=" + Path.Combine(Directory.GetCurrentDirectory(), "Casamento.db"));
        }

         public DbSet<Convidado> Convidado { get; set; }
    }
}