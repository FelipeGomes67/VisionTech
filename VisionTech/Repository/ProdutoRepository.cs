using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using VisionTech.Interface;
using VisionTech.Models;
using VisionTech.VisionTechBd;
using static System.Net.WebRequestMethods;

namespace VisionTech.Repository;

public class ProdutoRepository : IProdutoRepository
{
    private readonly VisionTechContext _context;

    public ProdutoRepository(VisionTechContext context)
    {
        _context = context;
    }

    public void AtualizarIdCorpo(Produto produtoAtualizado)
    {
        try
        {
            Produto produtoBuscado = _context.Produtos.Find(produtoAtualizado.IdProduto)!;
            if (produtoBuscado != null)
            {
                produtoBuscado.Nome = produtoAtualizado.Nome;
                produtoBuscado.IdProduto = produtoAtualizado.IdProduto;
            }
            _context.Produtos.Update(produtoBuscado!);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void AtualizarIdUrl(Guid id, Produto produtoAtualizado)
    {
        try
        {
            Produto produtoBuscado = _context.Produtos.Find(id.ToString())!;

            if (produtoBuscado != null)
            {
                produtoBuscado.Nome = produtoAtualizado.Nome;
                produtoBuscado.IdProduto = produtoAtualizado.IdProduto;
            }
            _context.Produtos.Update(produtoBuscado!);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public Produto BuscarPorId(Guid id)
    {
        try
        {
            Produto produtoBuscado = _context.Produtos.Find(id.ToString())!;
            return produtoBuscado;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Cadastrar(Produto novoProduto)
    {
        try
        {
            novoProduto.IdProduto = Guid.NewGuid().ToString();

            _context.Produtos.Add(novoProduto);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Deletar(Guid id)
    {
        try
        {
            Produto produtoBuscado = _context.Produtos.Find(id.ToString())!;
            if (produtoBuscado != null)
            {
                _context.Produtos.Remove(produtoBuscado);
            }
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public List<Produto> Listar()
    {
        try
        {
            List<Produto> listaProdutos = _context.Produtos
                .Include(c => c.IdCategoriaNavigation)
                .ToList();

            return listaProdutos;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
