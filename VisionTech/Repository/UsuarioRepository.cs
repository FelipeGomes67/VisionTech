using Microsoft.AspNetCore.Mvc.Filters;
using VisionTech.Interface;
using VisionTech.Models;
using VisionTech.Utils;
using VisionTech.VisionTechBd;

namespace VisionTech.Repository;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly VisionTechContext _context;

    public UsuarioRepository(VisionTechContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Método que busca um usuário pelo e-mail e valida se a senha digitada confere com o hash criptografado salvo no banco
    /// </summary>
    /// <param name="email">E-mail do usuário tentando autenticação</param>
    /// <param name="senha">Senha em texto limpo informada pelo usuário</param>
    /// <returns>Retorna o objeto do usuário caso a autenticação seja bem-sucedida, ou null se as credenciais forem inválidas</returns>
    public Usuario BuscarPorEmailESenha(string email, string senha)
    {
        try
        {
            Usuario usuarioBuscado = _context.Usuarios.FirstOrDefault(u => u.Email == email)!;

            if (usuarioBuscado != null)
            {
                bool confere = Criptografia.CompararHash(senha, usuarioBuscado.Senha);

                if (confere)
                {
                    return usuarioBuscado;
                }
            }
            return null!;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que busca um usuário por Id
    /// </summary>
    /// <param name="id">Id do usuário a ser buscado</param>
    /// <returns>Retorna o objeto do usuário buscado</returns>
    public Usuario BuscarPorId(Guid id)
    {
        try
        {
            Usuario usuarioBuscado = _context.Usuarios.Find(id.ToString())!;
            return usuarioBuscado;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que realiza o cadastro de um novo usuário no banco de dados, gerando um novo Guid e criptografando a senha em formato Hash
    /// </summary>
    /// <param name="novoUsuario">Objeto do usuário contendo as informações para o cadastro</param>
    public void Cadastrar(Usuario novoUsuario)
    {
        try
        {
            novoUsuario.IdUsuario = Guid.NewGuid().ToString();

            novoUsuario.Senha = Criptografia.GerarHash(novoUsuario.Senha!);

            _context.Usuarios.Add(novoUsuario);
            _context.SaveChanges();
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que lista todos os usuários cadastrados no banco de dados
    /// </summary>
    /// <returns>Retorna uma lista contendo todos os objetos de usuários encontrados</returns>
    public List<Usuario> Listar()
    {
        try
        {
            List<Usuario> listaUsuarios = _context.Usuarios.ToList();
            return listaUsuarios;
        }
        catch (Exception)
        {
            throw;
        }
    }

    /// <summary>
    /// Método que remove um usuário do banco de dados com base no Id fornecido
    /// </summary>
    /// <param name="id">Id do usuário que será deletado</param>
    public void Deletar(Guid id)
    {
        try
        {
            Usuario? usuarioBuscado = _context.Usuarios.Find(id.ToString());

            if (usuarioBuscado != null)
            {
                _context.Usuarios.Remove(usuarioBuscado);
                _context.SaveChanges();
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
}