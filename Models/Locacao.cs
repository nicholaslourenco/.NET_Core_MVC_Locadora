using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Locadora.Models
{
    public class Locacao
    {
        [Key()]
        public int Id { get; set; }
        public DateTime DataLocacao { get; set; }
        public DateTime DataDevolucao { get; set; }
        public string NomeUsuario { get; set; }
        public string Telefone { get; set; }
        public bool Devolvido { get; set; }

        [ForeignKey("Filme")]
        public int FilmeId { get; set; }
        public virtual Filme Filme { get; set; }
    }
}