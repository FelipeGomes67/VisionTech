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
