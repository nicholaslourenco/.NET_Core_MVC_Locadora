using System;
using Locadora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Controllers
{
    public class FilmeController : Controller
    {

        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(Filme filme)
        {
            byte[] imgdata = System.IO.File.ReadAllBytes("wwwroot/images/imgTeste.jpeg");
            filme.Imagem = imgdata;
            Console.WriteLine(imgdata);
            FilmesService filmesService = new FilmesService();

            if (filme.Id == 0)
            {
                filmesService.Inserir(filme);
            }
            else
            {
                filmesService.Editar(filme);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            FilmesService filmesService = new FilmesService();
            Filme filme = filmesService.BuscaId(id);
            return View(filme);
        }

        public IActionResult Excluir(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            return View(new FilmesService().BuscaId(id));
        }

        [HttpPost]
        public IActionResult Excluir(string decisao, int id)
        {
            if (decisao == "EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão do filme " + new FilmesService().BuscaId(id).Nome + " realizada com sucesso";
                new FilmesService().Excluir(id);
                return RedirectToAction("Index", "Home", new FilmesService().Listar());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão cancelada";
                return RedirectToAction("Index", "Home", new FilmesService().Listar());
            }
        }
    }
}