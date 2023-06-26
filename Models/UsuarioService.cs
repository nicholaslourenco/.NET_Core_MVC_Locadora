using System.Collections.Generic;
using System.Linq;

namespace Locadora.Models
{
    public class UsuarioService
    {
        public List<Usuario> Listar()
        {
            using (Context context = new Context())
            {
                return context.Usuarios.ToList();
            }
        }

        public void Inserir(Usuario usuario)
        {
            using (Context context = new Context())
            {
                context.Usuarios.Add(usuario);
                context.SaveChanges();
            }
        }

        public Usuario BuscaId(int id)
        {
            using (Context context = new Context())
            {
                return context.Usuarios.Find(id);
            }
        }

        public void Editar(Usuario usuarioEditado)
        {
            using (Context context = new Context())
            {
                Usuario usuarioAntigo = context.Usuarios.Find(usuarioEditado.Id);

                usuarioAntigo.Nome = usuarioEditado.Nome;
                usuarioAntigo.Login = usuarioEditado.Login;
                usuarioAntigo.Senha = usuarioEditado.Senha;
                usuarioAntigo.Tipo = usuarioEditado.Tipo;

                context.SaveChanges();
            }
        }

        public void Excluir(int id)
        {
            using (Context context = new Context())
            {
                context.Usuarios.Remove(context.Usuarios.Find(id));
                context.SaveChanges();
            }
        }
    }
}