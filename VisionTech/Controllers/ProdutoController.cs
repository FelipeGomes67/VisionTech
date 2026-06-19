using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VisionTech.DTO;
using VisionTech.Interface;
using VisionTech.Models;

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

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de buscar um produto por ID
    /// </summary>
    /// <param name="id">Id do produto buscado</param>
    /// <returns>Status code 200 e o produto buscado</returns>
    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        try
        {
            var produto = _produtoRepository.BuscarPorId(id);
            if (produto == null)
                return NotFound("Produto não encontrado!");

            return Ok(produto);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de listagem de produtos
    /// </summary>
    /// <returns>Status code 200 e a lista de produtos</returns>
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

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de cadastrar um produto, realizando o upload de imagem
    /// </summary>
    /// <param name="novoProduto">Dados e arquivo de imagem do produto a ser cadastrado</param>
    /// <returns>Status code 201 em caso de sucesso</returns>
    [HttpPost]
    public async Task<IActionResult> Post([FromForm] ProdutoDTO novoProduto)
    {
        if (String.IsNullOrWhiteSpace(novoProduto.Nome) || novoProduto.IdCategoria == null)
            return BadRequest("É obrigatório que o produto tenha nome e Categoria válida.");

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
        produto.QuantidadeEstoque = novoProduto.QuantidadeEstoque;

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

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de atualizar um produto enviando o ID pela URL e dados via Form
    /// </summary>
    /// <param name="id">Id do produto a ser atualizado</param>
    /// <param name="produto">Novos dados e nova imagem (opcional) do produto</param>
    /// <returns>Status code 204 (NoContent)</returns>
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

        // Atualiza o estoque se um novo valor for enviado no Form
        produtoBuscado.QuantidadeEstoque = produto.QuantidadeEstoque;

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

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de atualizar um produto
    /// </summary>
    /// <param name="produtoAtualizado">Objeto do produto com os dados atualizados</param>
    /// <returns>Status code 204 (NoContent)</returns>
    [HttpPut]
    public IActionResult PutBody([FromBody] Produto produtoAtualizado)
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

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de deletar um produto
    /// </summary>
    /// <param name="id">Id do produto a ser deletado</param>
    /// <returns>Status code 204 (NoContent)</returns>
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