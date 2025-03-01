using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using loopy.Models;
using loopy.Services;
using loopy.Data;

namespace loopy.Controllers;

[Route("productos")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _productoService;

    public ProductosController(ProductoService productoService)
    {
        _productoService = productoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Producto>>> GetAll()
    {
        return await _productoService.GetAllProductosAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> GetById(int id)
    {
        var producto = await _productoService.GetProductoByIdAsync(id);
        if (producto == null)
            return NotFound(new { message = "Producto no encontrado." });
        return producto;
    }

    [HttpPost("crear")]
    public async Task<ActionResult> CrearProducto(
        [FromForm] string nombre,
        [FromForm] int cantidad,
        [FromForm] decimal precio,
        [FromForm] int categoria,
        [FromForm] IFormFile imagen,
        [FromServices] ApplicationDbContext context) // Inyectar el contexto de la base de datos
    {
        var categoriaid = await context.Categorias.FindAsync(categoria);

        if (categoriaid == null)
        {
            return BadRequest("La categoría especificada no existe.");
        }

        var producto = new Producto
        {
            Nombre = nombre,
            Cantidad = cantidad,
            Precio = precio,
            CategoriaId = categoria
        };

        if (imagen != null)
        {
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            Directory.CreateDirectory(uploads);
            var filePath = Path.Combine(uploads, imagen.FileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }

            producto.ImagenUrl = "/images/" + imagen.FileName;
        }
        else
        {
            Console.WriteLine("No se recibió imagen.");
        }

        context.Productos.Add(producto);

        await context.SaveChangesAsync();

        return Ok(new { mensaje = "Producto creado exitosamente", producto });
    }


    [HttpPut("actualizar/{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Producto producto)
    {
        producto.Id = id;
        var updated = await _productoService.UpdateProductoAsync(producto);
        if (!updated)
            return NotFound(new { message = "No se pudo actualizar el producto." });
        return Ok(new { message = "Producto actualizado correctamente." });
    }

    [HttpDelete("eliminar/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _productoService.DeleteProductoAsync(id);
        if (!deleted)
            return NotFound(new { message = "No se pudo eliminar el producto." });
        return Ok(new { message = "Producto eliminado correctamente." });
    }
}
