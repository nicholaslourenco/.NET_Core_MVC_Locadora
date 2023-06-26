using System.Collections.Generic;

namespace Locadora.Models
{
    public class CadLocacaoViewModel
    {
        public ICollection<Filme> Filmes { get; set; }
        public Locacao Locacao { get; set; }
    }
}