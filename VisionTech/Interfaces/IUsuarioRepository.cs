using VisionTech.Models;

namespace VisionTech.Interface;

public interface IUsuarioRepository
{
    void Cadastrar(Usuario novoUsuario);
    Usuario BuscarPorId(Guid id);
    Usuario BuscarPorEmailESenha(string email, string senha);
    List<Usuario> Listar();
    void Deletar(Guid id);

}
