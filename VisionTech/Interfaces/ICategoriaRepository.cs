using VisionTech.Models;
using static System.Net.WebRequestMethods;

namespace VisionTech.Interface;

public interface ICategoriaRepository
{
    void Cadastrar(Categoria novaCategoria);
    List<Categoria> Listar();
    void AtualizarIdCorpo(Categoria generoAtualizado);
    void AtualizarIdUrl(Guid id, Categoria generoAtualizado);
    void Deletar(Guid id);
    Categoria BuscarPorId(Guid id);

}
