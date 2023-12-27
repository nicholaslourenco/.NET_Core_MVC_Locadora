using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using Locadora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Controllers
{
    public class FilmeController : Controller
    {

        [HttpGet]
        public IActionResult Cadastro()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            return View();
        }

        [HttpPost]
        public IActionResult Cadastro(FilmeViewModel filme, IFormFile file)
        {
            string extensao = Path.GetExtension(file.FileName);
            string[] extensoesValidas = new string[] { "jpg", "png", "jpeg" };
            if (!extensoesValidas.Contains(extensao))
            {
                ViewData["Message"] = "Extensão de arquivo não suportada, utilize imagens .jpg ou .png";
            }
            var ms = new MemoryStream();
            file.CopyTo(ms);
            var fileBytes = ms.ToArray();
            string imgBase64 = Convert.ToBase64String(fileBytes);
            filme.CaminhoImagem = imgBase64;
            filme.Imagem = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                //HTTP POST
                var postTask = client.PostAsJsonAsync<FilmeViewModel>("Filme", filme);
                postTask.Wait();
                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "Erro no Servidor.");
            return View(filme);
        }

        public IActionResult Edicao(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            FilmeViewModel filme = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/Filme/");

                // HTTP GET
                var responseTask = client.GetAsync("id?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<FilmeViewModel>();
                    readTask.Wait();

                    filme = readTask.Result;
                }
            }

            return View(filme);
        }

        [HttpPost]
        public IActionResult Edicao(FilmeViewModel filme, IFormFile? file)
        {
            if (file != null)
            {
                string extensao = Path.GetExtension(file.FileName);
                string[] extensoesValidas = new string[] { "jpg", "png", "jpeg" };
                if (!extensoesValidas.Contains(extensao))
                {
                    ViewData["Message"] = "Extensão de arquivo não suportada, utilize imagens .jpg ou .png";
                }
                var ms = new MemoryStream();
                file.CopyTo(ms);
                var fileBytes = ms.ToArray();
                string imgBase64 = Convert.ToBase64String(fileBytes);
                filme.CaminhoImagem = imgBase64;
                filme.Imagem = null;
            }
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/Filme/");

                // HTTP PUT
                var putTask = client.PutAsJsonAsync<FilmeViewModel>(filme.Id.ToString(), filme);
                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(filme);
        }


        public IActionResult Excluir(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            FilmeViewModel filme = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                // HTTP DELETE
                var deleteTask = client.DeleteAsync("Filme/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(filme);
        }
    }
}