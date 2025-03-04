using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using loopy.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using loopy.Services;

var builder = WebApplication.CreateBuilder(args);

// Configurar la conexión a la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login"; 
        options.LogoutPath = "/Auth/Logout";
    });

builder.Services.AddAuthorization();

// Agregar servicios al contenedor
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IStatsService, StatsService>();
builder.Services.AddScoped<ProductoService>();
builder.Services.AddScoped<CategoriaService>();

builder.Services.AddHttpContextAccessor();

// Configurar Session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

// Configurar CORS antes de `builder.Build()`
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// Registrar controladores con vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Manejo de errores y seguridad en producción
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

// Habilitar CORS antes de autenticación/autorización
app.UseCors("AllowAll");

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

// Configurar rutas correctamente
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

app.MapControllers();

app.Run();
