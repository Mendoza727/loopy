using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using loopy.Services;
using loopy.Models;

[Route("productos")]
[ApiController]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _productoService;
    public ProductosController(ProductoService productoService)
    {
        _productoService = productoService;
    }
    // Obtener todos los productos
    [HttpGet]
    public async Task<ActionResult<List<Producto>>> GetAll()
    {
        return await _productoService.GetAllProductosAsync();
    }
    // Obtener un producto por ID
    [HttpGet("{id}")]
    public async Task<ActionResult<Producto>> GetById(int id)
    {
        var producto = await _productoService.GetProductoByIdAsync(id);
        if (producto == null)
            return NotFound(new { message = "Producto no encontrado." });
        return producto;
    }
    // Crear un producto con imagen
    [HttpPost("crear-producto")]
    public async Task<ActionResult> Create([FromForm] Producto producto, [FromForm] IFormFile imagen)
    {
        if (imagen != null)
        {
            // Guardar la imagen en un directorio local (o en un servicio de almacenamiento)
            var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
            var filePath = Path.Combine(uploads, imagen.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imagen.CopyToAsync(stream);
            }
            // Guardar la ruta de la imagen en el producto
            producto.ImagenUrl = "/images/" + imagen.FileName;
        }
        var created = await _productoService.CreateProductoAsync(producto);
        if (!created)
            return BadRequest(new { message = "Error al crear el producto." });
        return Ok(new { message = "Producto creado correctamente." });
    }
    // Actualizar un producto
    [HttpPut("actualizar/{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Producto producto)
    {
        producto.Id = id;
        var updated = await _productoService.UpdateProductoAsync(producto);
        if (!updated)
            return NotFound(new { message = "No se pudo actualizar el producto." });
        return Ok(new { message = "Producto actualizado correctamente." });
    }
    // Eliminar un producto
    [HttpDelete("eliminar/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _productoService.DeleteProductoAsync(id);
        if (!deleted)
            return NotFound(new { message = "No se pudo eliminar el producto." });
        return Ok(new { message = "Producto eliminado correctamente." });
    }
}
