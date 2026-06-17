using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionTech.Interface;
using VisionTech.Models;

namespace VisionTech.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriaController : ControllerBase
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CategoriaController(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de buscar uma categoria por ID
    /// </summary>
    /// <param name="id">Id da categoria buscada</param>
    /// <returns>Status code 200 e a categoria buscada</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            return Ok(_categoriaRepository.BuscarPorId(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de listagem de categorias
    /// </summary>
    /// <returns>Status code 200 e a lista de categorias</returns>
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Ok(_categoriaRepository.Listar());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de cadastrar uma nova categoria
    /// </summary>
    /// <param name="novaCategoria">Objeto da categoria a ser cadastrada</param>
    /// <returns>Status code 201 em caso de sucesso</returns>
    [HttpPost]
    public IActionResult Post(Categoria novaCategoria)
    {
        try
        {
            _categoriaRepository.Cadastrar(novaCategoria);
            return StatusCode(201);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de atualizar uma categoria passando o ID pela URL
    /// </summary>
    /// <param name="id">Id da categoria a ser atualizada</param>
    /// <param name="categoriaAtualizada">Objeto com os novos dados da categoria</param>
    /// <returns>Status code 204 (NoContent)</returns>
    [HttpPut("{id}")]
    public IActionResult Put(Guid id, Categoria categoriaAtualizada)
    {
        try
        {
            _categoriaRepository.AtualizarIdUrl(id, categoriaAtualizada);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de atualizar uma categoria recebendo o objeto completo no corpo da requisição
    /// </summary>
    /// <param name="categoriaAtualizada">Objeto da categoria com os dados atualizados</param>
    /// <returns>Status code 204 (NoContent)</returns>
    [HttpPut]
    public IActionResult PutBody(Categoria categoriaAtualizada)
    {
        try
        {
            _categoriaRepository.AtualizarIdCorpo(categoriaAtualizada);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de deletar uma categoria por ID
    /// </summary>
    /// <param name="id">Id da categoria a ser deletada</param>
    /// <returns>Status code 204 (NoContent)</returns>
    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        try
        {
            _categoriaRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}