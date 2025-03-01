using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using loopy.Data;
using loopy.Models;

namespace loopy.Services
{
    public class CategoriaService
    {
        private readonly ApplicationDbContext _context;

        public CategoriaService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todas las categorías
        public async Task<List<Categoria>> GetAllCategoriasAsync()
        {
            return await _context.Categorias.Include(c => c.Productos).ToListAsync();
        }

        // Obtener una categoría por ID
        public async Task<Categoria> GetCategoriaByIdAsync(int id)
        {
            return await _context.Categorias.Include(c => c.Productos)
                                            .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Crear una nueva categoría
        public async Task<bool> CreateCategoriaAsync(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            return await _context.SaveChangesAsync() > 0;
        }

        // Actualizar una categoría existente
        public async Task<bool> UpdateCategoriaAsync(Categoria categoria)
        {
            var existingCategoria = await _context.Categorias.FindAsync(categoria.Id);
            if (existingCategoria == null)
                return false;

            existingCategoria.Nombre = categoria.Nombre;

            return await _context.SaveChangesAsync() > 0;
        }

        // Eliminar una categoría
        public async Task<bool> DeleteCategoriaAsync(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
                return false;

            _context.Categorias.Remove(categoria);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
