using System.ComponentModel.DataAnnotations;

namespace VisionTech.DTO;

public class LoginDTO
{
    [Required(ErrorMessage = "O email do usuário é obrigatório!")]
    public string? Email { get; set; }

    [Required(ErrorMessage = "A senha do usuário é obrigatória!")]
    public string? Senha { get; set; }
}
