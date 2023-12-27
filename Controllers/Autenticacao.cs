using System.Collections.Generic;
using System.Linq;
using Locadora.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Locadora.Controllers
{
    public class Autenticacao
    {
        public static void CheckLogin(Controller controller)
        {
            if (string.IsNullOrEmpty(controller.HttpContext.Session.GetString("Login")))
            {
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }

        public static bool verificacaoLoginSenha(string login, string senha, Controller controller)
        {
            using (var client = new HttpClient())
            {
                verificaAdmin(); // Problema Chato

                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                //HTTP GET
                var responseTask = client.GetAsync("Usuario");
                responseTask.Wait();
                var result = responseTask.Result;

                IEnumerable<UsuarioViewModel> users; // Problema
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<UsuarioViewModel>>();
                    readTask.Wait();
                    users = readTask.Result; // Problema
                }
                else
                {
                    users = null; // Problema
                    Console.WriteLine("Erro no Servidor");
                }

                IEnumerable<UsuarioViewModel> UsuarioEncontrado = users.Where(u => u.Login == login && u.Senha == senha); // Problema
                List<UsuarioViewModel> ListaUsuarioEncontrado = UsuarioEncontrado.ToList();

                if (ListaUsuarioEncontrado.Count == 0)
                {
                    return false;
                }
                else
                {
                    controller.HttpContext.Session.SetString("Login", ListaUsuarioEncontrado[0].Login);
                    controller.HttpContext.Session.SetString("Nome", ListaUsuarioEncontrado[0].Nome);
                    controller.HttpContext.Session.SetInt32("Tipo", ListaUsuarioEncontrado[0].Tipo);
                    return true;
                }
            }
        }

        public static void verificaAdmin()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost:5291/locadora/api/");

                //HTTP GET
                var responseTask = client.GetAsync("Usuario");
                responseTask.Wait();
                var result = responseTask.Result;

                IEnumerable<UsuarioViewModel> users; // Problema
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<IEnumerable<UsuarioViewModel>>();
                    readTask.Wait();
                    users = readTask.Result; // Problema
                }
                else
                {
                    users = null; // Problema
                    Console.WriteLine("Erro no Servidor");
                }

                IEnumerable<UsuarioViewModel> userEncontrado = users.Where(u => u.Login == "admin");

                if (userEncontrado.ToList().Count == 0)
                {
                    UsuarioViewModel admin = new UsuarioViewModel();
                    admin.Login = "admin";
                    admin.Senha = "123";
                    admin.Tipo = UsuarioViewModel.admin;
                    admin.Nome = "Administrator";

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<UsuarioViewModel>("Usuario", admin);
                    postTask.Wait();
                    var resultPost = postTask.Result;

                    if (result.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Cadastro do Usuário Admin Realizado com Sucesso!");
                    }
                }
            }
        }

        public static void verificaTipoUser(Controller controller)
        {
            if (!(controller.HttpContext.Session.GetInt32("Tipo") == UsuarioViewModel.admin))
            {
                controller.Request.HttpContext.Response.Redirect("/Usuario/NeedAdmin");
            }
        }
    }
}