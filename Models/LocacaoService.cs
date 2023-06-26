using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace Locadora.Models
{
    public class LocacaoService
    {
        public ICollection<Locacao> Listar(FiltroLocacao filtro = null)
        {
            using (Context context = new Context())
            {
                IQueryable<Locacao> locacaos;

                if (filtro != null)
                {
                    switch (filtro.TipoFiltro)
                    {
                        case "NomeUsuario":
                            locacaos = context.Locacaos.Where(l => l.NomeUsuario.Contains(filtro.Filtro)).Include(l => l.Filme);
                        break;
                        case "Filme":
                            locacaos = context.Locacaos.Include(l => l.Filme).Where(l => l.Filme.Nome.Contains(filtro.Filtro));
                        break;
                        default:
                            locacaos = context.Locacaos.Include(l => l.Filme);
                        break;
                    }
                }
                else
                {
                    locacaos = context.Locacaos.Include(l => l.Filme);
                }

                return locacaos.OrderBy(l => l.NomeUsuario).ToList();
            }
        }
        public void Inserir(Locacao locacao)
        {
            using (Context context = new Context())
            {
                context.Locacaos.Add(locacao);
                context.SaveChanges();
            }
        }
        public Locacao BuscaId(int id)
        {
            using (Context context = new Context())
            {
                return context.Locacaos.Find(id);
            }
        }
        public void Editar(Locacao locacaoEditada)
        {
            using (Context context = new Context())
            {
                Locacao locacaoAntiga = context.Locacaos.Find(locacaoEditada.Id);

                locacaoAntiga.DataLocacao = locacaoEditada.DataLocacao;
                locacaoAntiga.DataDevolucao = locacaoEditada.DataDevolucao;
                locacaoAntiga.Devolvido = locacaoEditada.Devolvido;
                locacaoAntiga.FilmeId = locacaoEditada.FilmeId;

                context.SaveChanges();
            }
        }
        public void Excluir(int id)
        {
            using (Context context = new Context())
            {
                context.Locacaos.Remove(context.Locacaos.Find(id));
                context.SaveChanges();
            }
        }
    }
}