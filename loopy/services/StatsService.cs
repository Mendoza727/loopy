using loopy.Data;
using Microsoft.EntityFrameworkCore;

public interface IStatsService
{
    Task<int> GetTotalProductosAsync();
    Task<int> GetTotalUsuariosAsync();
    Task<decimal> GetTotalCategorias();
}

public class StatsService : IStatsService
{
    private readonly ApplicationDbContext _context;

    public StatsService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalProductosAsync()
    {
        return await _context.Productos.CountAsync();
    }

    public async Task<int> GetTotalUsuariosAsync()
    {
        return await _context.Usuarios.CountAsync();
    }

    public async Task<decimal> GetTotalCategorias()
    {
        return await _context.Categorias.CountAsync();
    }
}
