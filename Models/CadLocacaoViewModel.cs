using System.Collections.Generic;

namespace Locadora.Models
{
    public class CadLocacaoViewModel
    {
        public IEnumerable<FilmeViewModel> Filmes { get; set; }
        public LocacaoViewModel Locacao { get; set; }
    }
}