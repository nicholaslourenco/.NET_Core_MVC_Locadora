using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Locadora.Models;
using System.Drawing;

namespace Locadora.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index(string tipoFiltro, string filtro, string itensPorPagina, int numDaPagina, int paginaAtual)
    {
        FiltroFilme filtroFilme = null;

        if (!string.IsNullOrEmpty(filtro))
        {
            filtroFilme = new FiltroFilme();
            filtroFilme.Filtro = filtro;
            filtroFilme.TipoFiltro = tipoFiltro;
        }

        ViewData["filmesPorPagina"] = (string.IsNullOrEmpty(itensPorPagina) ? 10 : Int32.Parse(itensPorPagina));
        ViewData["paginaAtual"] = (paginaAtual != 0 ? paginaAtual : 1);

        FilmesService filmesService = new FilmesService();
        List<Filme> listaDeFilmes = filmesService.Listar(filtroFilme);
        //Console.WriteLine(listaDeFilmes[0].Nome);
        for (int i = 0; i < listaDeFilmes.Count; i++)
        {
            string imgBase64Dados = Convert.ToBase64String(listaDeFilmes[i].Imagem);
            string imagemDadosURL = string.Format("data:image/png;base64,{0}", imgBase64Dados);
            listaDeFilmes[i].CaminhoImagem = imagemDadosURL;
        }
        return View(listaDeFilmes);
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
