using System.Net;
using Locadora.Models;
using Microsoft.AspNetCore.Mvc;

namespace Locadora.Controllers
{
    public class UsuarioController : Controller
    {
        //Funções Adicionais--------------------------------------------------------------

        public IActionResult Sair()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult NeedAdmin()
        {
            Autenticacao.CheckLogin(this);
            return View();
        }

        //Cadastro------------------------------------------------------------------------

        [HttpGet]
        public IActionResult CadastroUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroUser(UsuarioViewModel usuario)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");
                //HTTP POST
                var postTask = client.PostAsJsonAsync<UsuarioViewModel>("Usuario", usuario);
                postTask.Wait();
                var result = postTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("CadastroRealizado");
                }
            }
            ModelState.AddModelError(string.Empty, "Erro no Servidor.");
            return View(usuario);
        }

        public IActionResult CadastroRealizado()
        {
            return View();
        }

        //Listagem------------------------------------------------------------------------

        public IActionResult ListaDeUsuarios()
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                //HTTP GET
                var responseTask = client.GetAsync("Usuario");
                responseTask.Wait();
                var result = responseTask.Result;

                List<UsuarioViewModel> users;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<List<UsuarioViewModel>>();
                    readTask.Wait();
                    users = readTask.Result;
                }
                else
                {
                    users = new List<UsuarioViewModel>();
                    ModelState.AddModelError(string.Empty, "Erro no Servidor.");
                }
                return View(users);
            }
        }

        //Edição--------------------------------------------------------------------------

        public IActionResult EdicaoUser(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            UsuarioViewModel user = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/Usuario/");

                // HTTP GET
                var responseTask = client.GetAsync("id?id=" + id.ToString());
                responseTask.Wait();
                var result = responseTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<UsuarioViewModel>();
                    readTask.Wait();

                    user = readTask.Result;
                }
            }

            return View(user);
        }

        [HttpPost]
        public IActionResult EdicaoUser(UsuarioViewModel usuarioEditar)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/Usuario/");

                // HTTP PUT
                var putTask = client.PutAsJsonAsync<UsuarioViewModel>(usuarioEditar.Id.ToString(), usuarioEditar);
                putTask.Wait();
                var result = putTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListaDeUsuarios");
                }
            }
            return View(usuarioEditar);
        }

        //Exclusão------------------------------------------------------------------------


        public IActionResult ExcluirUser(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);
            UsuarioViewModel user = null;

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                // HTTP DELETE
                var deleteTask = client.DeleteAsync("Usuario/" + id.ToString());
                deleteTask.Wait();
                var result = deleteTask.Result;

                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("ListaDeUsuarios");
                }
            }
            return View(user);
        }
    }
}