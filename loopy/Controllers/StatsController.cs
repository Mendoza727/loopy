using Microsoft.AspNetCore.Mvc;

[Route("api/stats")]
[ApiController]
public class StatsController : ControllerBase
{
    private readonly IStatsService _statsService;

    public StatsController(IStatsService statsService)
    {
        _statsService = statsService;
    }

    [HttpGet("productos")]
    public async Task<IActionResult> GetTotalProductos()
    {
        var total = await _statsService.GetTotalProductosAsync();
        return Ok(new { total });
    }

    [HttpGet("usuarios")]
    public async Task<IActionResult> GetTotalUsuarios()
    {
        var total = await _statsService.GetTotalUsuariosAsync();
        return Ok(new { total });
    }

    [HttpGet("categorias")]
    public async Task<IActionResult> GetTotalCategorias()
    {
        var total = await _statsService.GetTotalCategorias();
        return Ok(new { total });
    }
}
