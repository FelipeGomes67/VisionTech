using Microsoft.AspNetCore.Http;

namespace VisionTech.DTO;

public class ProdutoDTO
{
    public string? Nome { get; set; }
    public Guid? IdCategoria { get; set; }
    public IFormFile? Imagem { get; set; }
    public int QuantidadeEstoque { get; set; } 
}