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

        public IActionResult CadastroUser()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CadastroUser(Usuario usuario)
        {
            usuario.Senha = Criptografo.TextoCriptografado(usuario.Senha);
            new UsuarioService().Inserir(usuario);
            return RedirectToAction("CadastroRealizado");
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

            return View(new UsuarioService().Listar());
        }

        //Edição--------------------------------------------------------------------------

        public IActionResult EdicaoUser(int id)
        {
            Autenticacao.CheckLogin(this);
            Usuario u = new UsuarioService().BuscaId(id);
            return View(u);
        }

        [HttpPost]
        public IActionResult EdicaoUser(Usuario usuarioEditar)
        {
            new UsuarioService().Editar(usuarioEditar);
            return RedirectToAction("ListaDeUsuarios");
        }

        //Exclusão------------------------------------------------------------------------

        public IActionResult ExcluirUser(int id)
        {
            Autenticacao.CheckLogin(this);
            Autenticacao.verificaTipoUser(this);

            return View(new UsuarioService().BuscaId(id));
        }

        [HttpPost]
        public IActionResult ExcluirUser(string decisao, int id)
        {
            if (decisao == "EXCLUIR")
            {
                ViewData["Mensagem"] = "Exclusão do Usuário" + new UsuarioService().BuscaId(id).Nome + "realizada com sucesso";
                new UsuarioService().Excluir(id);
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            }
            else
            {
                ViewData["Mensagem"] = "Exclusão Cancelada";
                return View("ListaDeUsuarios", new UsuarioService().Listar());
            }
        }
    }
}