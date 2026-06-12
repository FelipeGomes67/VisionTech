using VisionTech.Models;
using static System.Net.WebRequestMethods;

namespace VisionTech.Interface;

public interface IProdutoRepository
{
    void Cadastrar(Produto novoProduto);
    List<Produto> Listar();
    void AtualizarIdCorpo(Produto filmeAtualizado);
    void AtualizarIdUrl(Guid id, Produto filmeAtualizado);
    void Deletar(Guid id);
    Produto BuscarPorId(Guid id);
}
