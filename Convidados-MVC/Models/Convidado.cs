using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Convidados_MVC.Models
{
    public class Convidado
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public string TipoConvidado { get; set; }
        public string Confirmacao { get; set; }
        public string Padrinho { get; set; }
        public DateTime DataInclusao { get; set; }
        public string IdUsuario { get; set; }
        
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }
        public Convidado()
        {
            Id = Guid.NewGuid().ToString();
            DataInclusao = DateTime.Now;
            Confirmacao = "N";
            Padrinho = "N";
        }
    }
}
