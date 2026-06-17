using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using VisionTech.DTO;
using VisionTech.Interface;
using VisionTech.Models;

namespace VisionTech.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IUsuarioRepository _usuarioRepository;

    public LoginController(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    /// <summary>
    /// Endpoint da API que faz a chamada para o método de autenticação do usuário e gera o token JWT
    /// </summary>
    /// <param name="loginDto">Objeto contendo as credenciais de e-mail e senha do usuário</param>
    /// <returns>Status code 200 e o Token JWT gerado em caso de sucesso, ou 404 caso as credenciais estejam incorretas</returns>
    [HttpPost]
    public IActionResult Login(LoginDTO loginDto)
    {
        try
        {
            Usuario usuarioBuscado = _usuarioRepository.BuscarPorEmailESenha(loginDto.Email!, loginDto.Senha!);

            if (usuarioBuscado == null)
            {
                return NotFound("Email ou senha inválidos");
            }

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, usuarioBuscado.IdUsuario),
                new Claim(JwtRegisteredClaimNames.Email, usuarioBuscado.Email)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("visiontech-chave-autenticacao-webapi-dev"));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
            (
                issuer: "api_visiontech",
                audience: "api_visiontech",
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: creds
            );

            return Ok(new
            {
                token = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}