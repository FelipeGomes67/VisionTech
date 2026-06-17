using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionTech.DTO;
using VisionTech.Interface;
using VisionTech.Models;

namespace VisionTech.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UsuarioController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }


    /// <summary>
    /// Endpoint da API que faz a chamada para o método de lista de usuários
    /// </summary>
    /// <returns>Status code 200 e a lista de usuários</returns>
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Ok(_usuarioRepository.Listar());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de buscar um usuário
    /// </summary>
    /// <param name="id">id do usuário buscado</param>
    /// <returns>status code 200 e o usuário buscado</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            return Ok(_usuarioRepository.BuscarPorId(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de cadastro um usuário
    /// </summary>
    /// <param name="dto">Usuário a ser cadastrado</param>
    /// <returns>Status code 201 e o usuário a ser cadastrado</returns>
    [HttpPost]
    public IActionResult Cadastrar(UsuarioDTO dto)
    {
        try
        {
            Usuario novoUsuario = new Usuario
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Senha = dto.Senha
            };

            _usuarioRepository.Cadastrar(novoUsuario);

            return StatusCode(201, "Usuário cadastrado com sucesso!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de deletar um usuário
    /// </summary>
    /// <param name="id">Id do usuário</param>
    /// <returns>Status code 204</returns>
    [HttpDelete("{id}")]
    public IActionResult Deletar(Guid id)
    {
        try
        {
            Usuario usuarioBuscado = _usuarioRepository.BuscarPorId(id);

            if (usuarioBuscado == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            _usuarioRepository.Deletar(id);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

}

