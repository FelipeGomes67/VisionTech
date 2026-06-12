namespace VisionTech.DTO;

public class ProdutoDTO
{
    public string? Nome { get; set; }
    public IFormFile? Imagem { get; set; }
    public Guid? IdCategoria { get; set; }
}
