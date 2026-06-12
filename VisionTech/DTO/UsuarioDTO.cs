using System.ComponentModel.DataAnnotations;

namespace VisionTech.DTO;

public class UsuarioDTO
{
    [Required(ErrorMessage = "O nome é obrigatório.")]
    public string Nome { get; set; } = null!;

    [Required(ErrorMessage = "O e-mail é obrigatório.")]
    [EmailAddress(ErrorMessage = "E-mail em formato inválido.")]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "A senha é obrigatória.")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "A senha deve ter no mínimo 3 caracteres.")]
    public string Senha { get; set; } = null!;
}
