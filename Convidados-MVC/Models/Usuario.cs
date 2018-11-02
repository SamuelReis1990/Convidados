using System;

namespace Convidados_MVC.Models
{
    public class Usuario
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public DateTime DataInclusao { get; set; }
        public Usuario()
        {
            Id = Guid.NewGuid().ToString();
            DataInclusao = DateTime.Now;            
        }
    }
}
