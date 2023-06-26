using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Locadora.Models
{
    public class FilmesService
    {

        public List<Filme> Listar(FiltroFilme filtro = null)
        {
            using (Context context = new Context())
            {
                IQueryable<Filme> query;

                if (filtro != null)
                {
                    switch (filtro.TipoFiltro)
                    {
                        case "Nome":
                            query = context.Filmes.Where(f => f.Nome.Contains(filtro.Filtro));
                            break;
                        case "Genero":
                            query = context.Filmes.Where(f => f.Genero.Contains(filtro.Filtro));
                            break;
                        case "Classificacao":
                            query = context.Filmes.Where(f => f.Classificacao.Contains(filtro.Filtro));
                            break;
                        default:
                            query = context.Filmes;
                            break;
                    }
                }
                else
                {
                    query = context.Filmes;
                }

                return query.OrderBy(f => f.Nome).ToList();
            }
        }

        public ICollection<Filme> ListarDisponiveis()
        {
            using (Context context = new Context())
            {
                //busca os livros onde o id não está entre os ids de livro em empréstimo
                // utiliza uma subconsulta
                return
                    context.Filmes
                    .Where(f => !(context.Locacaos.Where(l => l.Devolvido == false).Select(l => l.FilmeId).Contains(f.Id)))
                    .ToList();
            }
        }

        public void Inserir(Filme filme)
        {
            using (Context context = new Context())
            {
                context.Filmes.Add(filme);
                context.SaveChanges();
            }
        }

        public Filme BuscaId(int id)
        {
            using (Context context = new Context())
            {
                return context.Filmes.Find(id);
            }
        }

        public void Editar(Filme filmeEditado)
        {
            using (Context context = new Context())
            {
                Filme filmeAntigo = context.Filmes.Find(filmeEditado.Id);

                filmeAntigo.Nome = filmeEditado.Nome;
                filmeAntigo.Genero = filmeEditado.Genero;
                filmeAntigo.Classificacao = filmeEditado.Classificacao;
                //filmeAntigo.Imagem = filmeEditado.Imagem;

                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            using (Context context = new Context())
            {
                context.Filmes.Remove(context.Filmes.Find(id));
                context.SaveChanges();
            }
        }
    }
}