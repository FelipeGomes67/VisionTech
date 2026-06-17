using Microsoft.AspNetCore.Mvc.Filters;
using VisionTech.Interface;
using VisionTech.Models;
using VisionTech.VisionTechBd;

namespace VisionTech.Repository;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly VisionTechContext _context;

    public CategoriaRepository(VisionTechContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Método que atualiza os dados de uma categoria recebendo o objeto completo no corpo da requisição
    /// </summary>
    /// <param name="categoriaAtualizada">Objeto contendo os dados atualizados e o ID da categoria</param>
    public void AtualizarIdCorpo(Categoria categoriaAtualizada)
    {
        try
        {
            Categoria categoriaBuscada = _context.Categoria.Find(categoriaAtualizada.IdCategoria)!;
            if (categoriaBuscada != null)
            {
                categoriaBuscada.Nome = categoriaAtualizada.Nome;
            }
            _context.Categoria.Update(categoriaBuscada!);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que atualiza os dados de uma categoria localizando-a pelo ID fornecido na URL
    /// </summary>
    /// <param name="id">Id da categoria a ser atualizada</param>
    /// <param name="categoriaAtualizada">Objeto contendo as novas informações da categoria</param>
    public void AtualizarIdUrl(Guid id, Categoria categoriaAtualizada)
    {
        try
        {
            Categoria categoriaBuscada = _context.Categoria.Find(id.ToString())!;

            if (categoriaBuscada != null)
            {
                categoriaBuscada.Nome = categoriaAtualizada.Nome;
            }
            _context.Categoria.Update(categoriaBuscada!);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que busca uma categoria por Id
    /// </summary>
    /// <param name="id">Id da categoria a ser buscada</param>
    /// <returns>Retorna o objeto da categoria buscada</returns>
    public Categoria BuscarPorId(Guid id)
    {
        try
        {
            Categoria categoriaBuscada = _context.Categoria.Find(id.ToString())!;
            return categoriaBuscada;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que realiza o cadastro de uma nova categoria no banco de dados, gerando um novo Guid
    /// </summary>
    /// <param name="novaCategoria">Objeto da categoria contendo as informações para o cadastro</param>
    public void Cadastrar(Categoria novaCategoria)
    {
        try
        {
            novaCategoria.IdCategoria = Guid.NewGuid().ToString();
            _context.Categoria.Add(novaCategoria);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que remove uma categoria do banco de dados com base no Id fornecido
    /// </summary>
    /// <param name="id">Id da categoria que será deletada</param>
    public void Deletar(Guid id)
    {
        try
        {
            Categoria categoriaBuscada = _context.Categoria.Find(id.ToString())!;
            if (categoriaBuscada != null)
            {
                _context.Categoria.Remove(categoriaBuscada);
            }
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que lista todas as categorias cadastradas no banco de dados
    /// </summary>
    /// <returns>Retorna uma lista contendo todos os objetos de categorias encontrados</returns>
    public List<Categoria> Listar()
    {
        try
        {
            List<Categoria> listaCategorias = _context.Categoria.ToList();
            return listaCategorias;
        }
        catch (Exception)
        {
            throw;
        }
    }
}