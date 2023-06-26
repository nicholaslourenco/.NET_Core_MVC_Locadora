using Locadora.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace Locadora.Controllers
{
    
    public class LocacaoController : Controller
    {
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            FilmesService filmesService = new FilmesService();
            LocacaoService emprestimoService = new LocacaoService();

            CadLocacaoViewModel cadModel = new CadLocacaoViewModel();
            cadModel.Filmes = filmesService.ListarDisponiveis();
            return View(cadModel);
        }

        [HttpPost]
        public IActionResult Cadastro(CadLocacaoViewModel viewModel)
        {
            LocacaoService locacaoService = new LocacaoService();
            
            if(viewModel.Locacao.Id == 0)
            {
                locacaoService.Inserir(viewModel.Locacao);
            }
            else
            {
                locacaoService.Editar(viewModel.Locacao);
            }
            return RedirectToAction("Listagem");
        }

        public IActionResult Listagem(string tipoFiltro, string filtro, string intensPorPagina, int numDaPagina, int paginaAtual)
        {
            Autenticacao.CheckLogin(this);
            FiltroLocacao empFiltro = null;
            if (!string.IsNullOrEmpty(filtro))
            {
                empFiltro = new FiltroLocacao();
                empFiltro.Filtro = filtro;
                empFiltro.TipoFiltro = tipoFiltro;
            }

                ViewData["emprestimosPorPagina"] = (string.IsNullOrEmpty(intensPorPagina) ? 10 : Int32.Parse(intensPorPagina));
                ViewData["paginaAtual"] = (paginaAtual != 0 ? paginaAtual : 1);

            LocacaoService service = new LocacaoService();
            return View(service.Listar(empFiltro));
        }

        public IActionResult Edicao(int id)
        {
            FilmesService filmesService = new FilmesService();
            LocacaoService locacaoService = new LocacaoService();
            Locacao locacao = locacaoService.BuscaId(id);

            CadLocacaoViewModel cadModel = new CadLocacaoViewModel();
            cadModel.Filmes = filmesService.ListarDisponiveis();
            cadModel.Locacao = locacao;
            
            return View(cadModel);
        }

        public IActionResult Excluir(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            return View(new LocacaoService().BuscaId(id));
        }

        [HttpPost]
        public IActionResult Excluir(string decisao, int id)
        {
            if (decisao == "EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão da locação no nome de" + new LocacaoService().BuscaId(id).NomeUsuario + " realizada com sucesso";
                new LocacaoService().Excluir(id);
                return RedirectToAction("Listagem", new LocacaoService().Listar());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão cancelada";
                return RedirectToAction("Listagem", new LocacaoService().Listar());
            }
        }
    }
}