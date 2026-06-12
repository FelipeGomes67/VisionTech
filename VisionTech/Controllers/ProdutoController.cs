using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionTech.DTO;
using VisionTech.Interface;
using VisionTech.Models;
using static System.Net.WebRequestMethods;

namespace VisionTech.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            return Ok(_produtoRepository.BuscarPorId(id));
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);

        }
    }



    //[Authorize]
    [HttpGet]
    public IActionResult Get()
    {
        try
        {
            return Ok(_produtoRepository.Listar());
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ProdutoDTO novoProduto)
    {
        if (String.IsNullOrWhiteSpace(novoProduto.Nome) || novoProduto.IdCategoria == null)
            return BadRequest("É obrigatório que o filme tenha nome e Gênero válido.");

        Produto produto = new Produto();

        if (novoProduto.Imagem != null && novoProduto.Imagem.Length > 0)
        {
            var extensao = Path.GetExtension(novoProduto.Imagem.FileName);
            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";

            var pastaRelativa = "wwwroot/imagens";
            var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), pastaRelativa);


            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);

            var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);

            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await novoProduto.Imagem.CopyToAsync(stream);
            }

            produto.Imagem = nomeArquivo;
        }

        produto.IdCategoria = novoProduto.IdCategoria.ToString();
        produto.Nome = novoProduto.Nome!;

        try
        {
            _produtoRepository.Cadastrar(produto);
            return StatusCode(201);
        }
        catch (Exception e)
        {
            var erroReal = e.InnerException?.Message ?? e.Message;
            return BadRequest(new { erro = "Erro no banco de dados", detalhes = erroReal });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(Guid id, [FromForm] ProdutoDTO produto)
    {
        var produtoBuscado = _produtoRepository.BuscarPorId(id);
        if (produtoBuscado == null)
            return NotFound("Produto não encontrado!");

        if (!String.IsNullOrWhiteSpace(produto.Nome))
            produtoBuscado.Nome = produto.Nome;

        if (produto.IdCategoria != null && produto.IdCategoria.ToString() != produtoBuscado.IdCategoria)
            produtoBuscado.IdCategoria = produto.IdCategoria.ToString();

        if (produto.Imagem != null && produto.Imagem.Length != 0)
        {
            var pastaRelativa = "wwwroot/imagens";
            var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), pastaRelativa);

            if (!String.IsNullOrEmpty(produtoBuscado.Imagem))
            {
                var caminhoAntigo = Path.Combine(caminhoPasta, produtoBuscado.Imagem);
                if (System.IO.File.Exists(caminhoAntigo))
                    System.IO.File.Delete(caminhoAntigo);
            }

            var extensao = Path.GetExtension(produto.Imagem.FileName);
            var nomeArquivo = $"{Guid.NewGuid()}{extensao}";

            if (!Directory.Exists(caminhoPasta))
                Directory.CreateDirectory(caminhoPasta);

            var caminhoCompleto = Path.Combine(caminhoPasta, nomeArquivo);
            using (var stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                await produto.Imagem.CopyToAsync(stream);
            }

            produtoBuscado.Imagem = nomeArquivo;
        }
        try
        {
            _produtoRepository.AtualizarIdUrl(id, produtoBuscado);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public IActionResult PutBody([FromBody] Produto produtoAtualizado) // 👈 ADICIONADO [FromBody] PARA ISOLAR O COMPORTAMENTO
    {
        try
        {
            _produtoRepository.AtualizarIdCorpo(produtoAtualizado);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        var produtoBuscado = _produtoRepository.BuscarPorId(id);
        if (produtoBuscado == null)
            return NotFound("Produto não encontrado!");

        var pastaRelativa = "wwwroot/imagens";
        var caminhoPasta = Path.Combine(Directory.GetCurrentDirectory(), pastaRelativa);

        if (!String.IsNullOrEmpty(produtoBuscado.Imagem))
        {
            var caminho = Path.Combine(caminhoPasta, produtoBuscado.Imagem);

            if (System.IO.File.Exists(caminho))
                System.IO.File.Delete(caminho);
        }

        try
        {
            _produtoRepository.Deletar(id);
            return NoContent();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}