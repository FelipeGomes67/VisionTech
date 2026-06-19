using Microsoft.EntityFrameworkCore;
using VisionTech.Interface;
using VisionTech.Models;
using VisionTech.VisionTechBd;

namespace VisionTech.Repository;

public class ProdutoRepository : IProdutoRepository
{
    private readonly VisionTechContext _context;

    public ProdutoRepository(VisionTechContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Método que atualiza os dados de um produto recebendo o objeto completo no corpo da requisição
    /// </summary>
    /// <param name="produtoAtualizado">Objeto contendo os dados atualizados e o ID do produto</param>
    public void AtualizarIdCorpo(Produto produtoAtualizado)
    {
        try
        {
            Produto produtoBuscado = _context.Produtos.Find(produtoAtualizado.IdProduto)!;

            if (produtoBuscado != null)
            {
                // Atualiza as propriedades necessárias (menos o ID)
                produtoBuscado.Nome = produtoAtualizado.Nome;
                produtoBuscado.Imagem = produtoAtualizado.Imagem;
                produtoBuscado.QuantidadeEstoque = produtoAtualizado.QuantidadeEstoque; // Refletindo a Opção 1 escolhida
                produtoBuscado.IdCategoria = produtoAtualizado.IdCategoria;

                _context.Produtos.Update(produtoBuscado);
                _context.SaveChanges();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que atualiza os dados de um produto localizando-o pelo ID fornecido na URL
    /// </summary>
    /// <param name="id">Id do produto a ser atualizado</param>
    /// <param name="produtoAtualizado">Objeto contendo as novas informações do produto</param>
    public void AtualizarIdUrl(Guid id, Produto produtoAtualizado)
    {
        try
        {
            Produto produtoBuscado = _context.Produtos.Find(id.ToString())!;

            if (produtoBuscado != null)
            {
                // Atualiza as propriedades necessárias (menos o ID)
                produtoBuscado.Nome = produtoAtualizado.Nome;
                produtoBuscado.Imagem = produtoAtualizado.Imagem;
                produtoBuscado.QuantidadeEstoque = produtoAtualizado.QuantidadeEstoque; // Refletindo a Opção 1 escolhida
                produtoBuscado.IdCategoria = produtoAtualizado.IdCategoria;

                _context.Produtos.Update(produtoBuscado);
                _context.SaveChanges();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que busca um produto por Id
    /// </summary>
    /// <param name="id">Id do produto a ser buscado</param>
    /// <returns>Retorna o objeto do produto buscado</returns>
    public Produto BuscarPorId(Guid id)
    {
        try
        {
            return _context.Produtos.Find(id.ToString())!;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que realiza o cadastro de um novo produto no banco de dados, gerando um novo Guid
    /// </summary>
    /// <param name="novoProduto">Objeto do produto contendo as informações para o cadastro</param>
    public void Cadastrar(Produto novoProduto)
    {
        try
        {
            // Garante a geração de um ID string válido baseado em Guid para o banco
            novoProduto.IdProduto = Guid.NewGuid().ToString();

            _context.Produtos.Add(novoProduto);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que remove um produto do banco de dados com base no Id fornecido
    /// </summary>
    /// <param name="id">Id do produto que será deletado</param>
    public void Deletar(Guid id)
    {
        try
        {
            Produto produtoBuscado = _context.Produtos.Find(id.ToString())!;
            if (produtoBuscado != null)
            {
                _context.Produtos.Remove(produtoBuscado);
                _context.SaveChanges();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que lista todos os produtos cadastrados no banco de dados, incluindo os dados de sua respectiva categoria
    /// </summary>
    /// <returns>Retorna uma lista contendo todos os objetos de produtos encontrados com os dados de categoria carregados</returns>
    public List<Produto> Listar()
    {
        try
        {
            return _context.Produtos
                .Include(c => c.IdCategoriaNavigation)
                .ToList();
        }
        catch (Exception)
        {
            throw;
        }
    }
}