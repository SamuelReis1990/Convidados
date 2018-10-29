using System;

namespace Convidados_MVC.Models
{
    public class Convidado
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string TipoConvidado { get; set; }
        public string Confirmacao { get; set; }
        public DateTime DataInclusao { get; set; }
        public Convidado()
        {
            Id = Guid.NewGuid().ToString();
            DataInclusao = DateTime.Now;
        }
    }
}
