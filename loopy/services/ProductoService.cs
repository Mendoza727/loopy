using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using loopy.Data;
using loopy.Models;

namespace loopy.Services
{
    public class ProductoService
    {
        private readonly ApplicationDbContext _context;

        public ProductoService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Obtener todos los productos
        public async Task<List<Producto>> GetAllProductosAsync()
        {
            return await _context.Productos.Include(p => p.Categoria).ToListAsync();
        }

        // Obtener un producto por ID
        public async Task<Producto> GetProductoByIdAsync(int id)
        {
            return await _context.Productos.Include(p => p.Categoria)
                                           .FirstOrDefaultAsync(p => p.Id == id);
        }

        // Crear un nuevo producto
        public async Task<bool> CreateProductoAsync(Producto producto)
        {
            _context.Productos.Add(producto);
            return await _context.SaveChangesAsync() > 0;
        }

        // Actualizar un producto existente
        public async Task<bool> UpdateProductoAsync(Producto producto)
        {
            var existingProducto = await _context.Productos.FindAsync(producto.Id);
            if (existingProducto == null)
                return false;

            existingProducto.Nombre = producto.Nombre;
            existingProducto.Cantidad = producto.Cantidad;
            existingProducto.Precio = producto.Precio;
            existingProducto.ImagenUrl = producto.ImagenUrl;
            existingProducto.CategoriaId = producto.CategoriaId;

            return await _context.SaveChangesAsync() > 0;
        }

        // Eliminar un producto
        public async Task<bool> DeleteProductoAsync(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
                return false;

            _context.Productos.Remove(producto);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
