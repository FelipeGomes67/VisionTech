using Microsoft.AspNetCore.Mvc.Filters;
using VisionTech.Interface;
using VisionTech.Models;
using VisionTech.VisionTechBd;

namespace VisionTech.Repository;


    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly VisionTechContext _context;
        public CategoriaRepository(VisionTechContext context)
        {
            _context = context;
        }
        public void AtualizarIdCorpo(Categoria categoriaAtualizada)
        {
            try
            {
                Categoria categoriaBuscada = _context.Categoria.Find(categoriaAtualizada.IdCategoria)!;
                if (categoriaBuscada != null)
                {
                    categoriaBuscada.Nome = categoriaAtualizada.Nome;
                }
                _context.Categoria.Update(categoriaBuscada!);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AtualizarIdUrl(Guid id, Categoria categoriaAtualizada)
        {
            try
            {
                Categoria categoriaBuscada = _context.Categoria.Find(id.ToString())!;

                if (categoriaBuscada != null)
                {
                    categoriaBuscada.Nome = categoriaAtualizada.Nome;
                }
                _context.Categoria.Update(categoriaBuscada!);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Categoria BuscarPorId(Guid id)
        {
            try
            {
                Categoria categoriaBuscada = _context.Categoria.Find(id.ToString())!;
                return categoriaBuscada;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Cadastrar(Categoria novaCategoria)
        {
            try
            {
                novaCategoria.IdCategoria = Guid.NewGuid().ToString();
                _context.Categoria.Add(novaCategoria);

                _context.SaveChanges();
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
                Categoria categoriaBuscada = _context.Categoria.Find(id.ToString())!;
                if (categoriaBuscada != null)
                {
                    _context.Categoria.Remove(categoriaBuscada);
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<Categoria> Listar()
        {
            try
            {
                List<Categoria> listaCategorias = _context.Categoria.ToList();
                return listaCategorias;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
