using loopy.Data;
using loopy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication;

using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

public interface IAuthService
{
    Task<bool> LoginAsync(string email, string password);
    Task<bool> RegisterAsync(string name, string email, string password, string imagenPerfilUrl);
    Task LogoutAsync();
}


public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> LoginAsync(string email, string password)
    {
        var user = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            return false;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.Nombre),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties { IsPersistent = true };

        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null)
        {
            throw new InvalidOperationException("HttpContext is null");
        }

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            authProperties
        );

        return true;
    }

    public async Task<bool> RegisterAsync(string name, string email, string password, string imagenPerfilUrl)
    {
        if (await _context.Usuarios.AnyAsync(u => u.Email == email))
            return false;

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        var user = new Usuario
        {
            Nombre = name,
            Email = email,
            PasswordHash = hashedPassword,
            ImagenPerfilUrl = imagenPerfilUrl
        };

        _context.Usuarios.Add(user);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task LogoutAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext != null)
        {
            await httpContext.SignOutAsync();
        }
    }
}