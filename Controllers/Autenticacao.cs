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
            if(string.IsNullOrEmpty(controller.HttpContext.Session.GetString("Login")))
            {
                controller.Request.HttpContext.Response.Redirect("/Home/Login");
            }
        }

        public static bool verificacaoLoginSenha(string login, string senha, Controller controller)
        {
            using (Context context = new Context())
            {
                verificaAdmin(context);
                senha = Criptografo.TextoCriptografado(senha);

                IQueryable<Usuario> UsuarioEncontrado = context.Usuarios.Where(u => u.Login == login && u.Senha == senha);
                List<Usuario> ListaUsuarioEncontrado = UsuarioEncontrado.ToList();

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

        public static void verificaAdmin(Context context)
        {
            IQueryable<Usuario> userEncontrado = context.Usuarios.Where(u => u.Login == "admin");

            if (userEncontrado.ToList().Count == 0)
            {
                Usuario admin = new Usuario();
                admin.Login = "admin";
                admin.Senha = Criptografo.TextoCriptografado("123");
                admin.Tipo = Usuario.admin;
                admin.Nome = "Administrator";

                context.Usuarios.Add(admin);
                context.SaveChanges();
            }
        }

        public static void verificaTipoUser(Controller controller)
        {
            if (!(controller.HttpContext.Session.GetInt32("Tipo") == Usuario.admin))
            {
                controller.Request.HttpContext.Response.Redirect("/Usuario/NeedAdmin");
            }
        }
    }
}