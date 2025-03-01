using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using loopy.Services;
using loopy.Models;

[Route("api")]
[ApiController]
public class CategoriasController : Controller
{
    private readonly CategoriaService _categoriaService;
    public CategoriasController(CategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    // Obtener la vista de categorias
    [HttpGet("category")]
    public IActionResult Category()
    {
        return View();
    }

    // Obtener todas las categorías
    [HttpGet]
    public async Task<ActionResult<List<Categoria>>> GetAll()
    {
        return await _categoriaService.GetAllCategoriasAsync();
    }
    // Obtener una categoría por ID
    [HttpGet("by-id/{id}")]
    public async Task<ActionResult<Categoria>> GetById(int id)
    {
        var categoria = await _categoriaService.GetCategoriaByIdAsync(id);
        if (categoria == null)
            return NotFound(new { message = "Categoría no encontrada." });
        return categoria;
    }
    // Crear una nueva categoría
    [HttpPost("create")]
    public async Task<ActionResult> Create([FromBody] Categoria categoria)
    {
        if (categoria == null || string.IsNullOrWhiteSpace(categoria.Nombre))
            return BadRequest(new { message = "El nombre de la categoría es obligatorio." });
    
        var created = await _categoriaService.CreateCategoriaAsync(categoria);
        if (!created)
            return BadRequest(new { message = "Error al crear la categoría." });
    
        return Ok(new { message = "Categoría creada correctamente." });
    }

    // Actualizar una categoría
    [HttpPut("update/{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Categoria categoria)
    {
        categoria.Id = id;
        var updated = await _categoriaService.UpdateCategoriaAsync(categoria);
        if (!updated)
            return NotFound(new { message = "No se pudo actualizar la categoría." });
        return Ok(new { message = "Categoría actualizada correctamente." });
    }
    // Eliminar una categoría
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _categoriaService.DeleteCategoriaAsync(id);
        if (!deleted)
            return NotFound(new { message = "No se pudo eliminar la categoría." });
        return Ok(new { message = "Categoría eliminada correctamente." });
    }
}

