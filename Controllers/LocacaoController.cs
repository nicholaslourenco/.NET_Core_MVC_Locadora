using Locadora.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;

namespace Locadora.Controllers
{
    public class LocacaoController : Controller
    {

        [HttpGet]
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            IEnumerable<FilmeViewModel> filmes = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                //HTTP GET
                var responseTask = client.GetAsync("Filme/tipoFiltro-filtro?tipoFiltro=null&filtro=null");
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IList<FilmeViewModel>>();
                    readTask.Wait();
                    filmes = readTask.Result;
                }
                else
                {
                    filmes = Enumerable.Empty<FilmeViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no Servidor.");
                }

                CadLocacaoViewModel cadModel = new CadLocacaoViewModel();
                cadModel.Filmes = filmes;
                return View(cadModel);

            }
        }

        [HttpPost]
        public IActionResult Cadastro(CadLocacaoViewModel viewModel)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");
                //HTTP POST
                var postTask = client.PostAsJsonAsync<LocacaoViewModel>("Locacao", viewModel.Locacao);
                postTask.Wait();
                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Listagem");
                }
            }
            ModelState.AddModelError(string.Empty, "Erro no Servidor");
            return RedirectToAction("Falha");
        }

        public IActionResult Listagem(string tipoFiltro, string filtro, string intensPorPagina, int numDaPagina, int paginaAtual)
        {
            using (var client = new HttpClient())
            {
                Autenticacao.CheckLogin(this);
                Autenticacao.verificaTipoUser(this);
                if (string.IsNullOrEmpty(tipoFiltro))
                {
                    tipoFiltro = "null";
                }
                if (string.IsNullOrEmpty(filtro))
                {
                    filtro = "null";
                }
                ViewData["emprestimosPorPagina"] = (string.IsNullOrEmpty(intensPorPagina) ? 10 : Int32.Parse(intensPorPagina));
                ViewData["paginaAtual"] = (paginaAtual != 0 ? paginaAtual : 1);

                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                //HTTP GET
                var responseTask = client.GetAsync($"Locacao/tipoFiltro-filtro?tipoFiltro={tipoFiltro}&filtro={filtro}");
                responseTask.Wait();
                var result = responseTask.Result;

                List<LocacaoViewModel> locacoes;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<LocacaoViewModel>>();
                    readTask.Wait();
                    locacoes = readTask.Result;
                }
                else
                {
                    locacoes = new List<LocacaoViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no Servidor.");
                }
                return View(locacoes);
            }
        }


        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            CadLocacaoViewModel locacao = new CadLocacaoViewModel();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                // HTTP GET Locações
                var responseTaskLoc = client.GetAsync("Locacao/id?id=" + id.ToString());
                responseTaskLoc.Wait();
                var resultLoc = responseTaskLoc.Result;

                if (resultLoc.IsSuccessStatusCode)
                {
                    var readTask = resultLoc.Content.ReadAsAsync<LocacaoViewModel>();
                    readTask.Wait();

                    locacao.Locacao = readTask.Result;
                }

                // HTTP GET Filmes
                var responseTaskFilm = client.GetAsync($"Filme/tipoFiltro-filtro?tipoFiltro=null&filtro=null");
                responseTaskFilm.Wait();
                var resultFilm = responseTaskFilm.Result;

                if (resultFilm.IsSuccessStatusCode)
                {
                    var readTask = resultFilm.Content.ReadAsAsync<IEnumerable<FilmeViewModel>>();
                    readTask.Wait();

                    locacao.Filmes = readTask.Result;
                }
            }

            return View(locacao);
        }

        [HttpPost]
        public IActionResult Edicao(CadLocacaoViewModel locacaoEditar)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/Locacao/");

                // HTTP PUT
                var putTask = client.PutAsJsonAsync<LocacaoViewModel>(locacaoEditar.Locacao.Id.ToString(), locacaoEditar.Locacao);
                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Listagem");
                }
            }
            return RedirectToAction("Falha");
        }

        
        public IActionResult Excluir(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            Console.WriteLine(id);
            LocacaoViewModel locacao = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                // HTTP DELETE
                var deleteTask = client.DeleteAsync("Locacao/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Listagem");
                }
            }
            return View(locacao);
        }

        public IActionResult Falha()
        {
            return View();
        }
    }
}
