using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Locadora.Models;
using System.Drawing;
using System.Collections.Generic;

namespace Locadora.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string tipoFiltro, string filtro, int paginaAtual, int numDaPagina, string itensPorPagina)
        {
            using (var client = new HttpClient())
            {
                if (string.IsNullOrEmpty(tipoFiltro))
                {
                    tipoFiltro = "null";
                }
                if (string.IsNullOrEmpty(filtro))
                {
                    filtro = "null";
                }
                ViewData["filmesPorPagina"] = (string.IsNullOrEmpty(itensPorPagina) ? 10 : Int32.Parse(itensPorPagina));
                ViewData["paginaAtual"] = (paginaAtual != 0 ? paginaAtual : 1);

                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                //HTTP GET
                var responseTask = client.GetAsync($"Filme/tipoFiltro-filtro?tipoFiltro={tipoFiltro}&filtro={filtro}");
                responseTask.Wait();
                var result = responseTask.Result;

                List<FilmeViewModel> filmes;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<FilmeViewModel>>();
                    readTask.Wait();
                    filmes = readTask.Result;
                }
                else
                {
                    filmes = new List<FilmeViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no Servidor.");
                }
                return View(filmes);
            }
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string login, string senha)
        {
            if (Autenticacao.verificacaoLoginSenha(login, senha, this))
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewData["Erro"] = "Senha inválida";
                return View();
            }
        }
    }

}