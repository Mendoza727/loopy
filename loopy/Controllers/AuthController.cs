using Microsoft.AspNetCore.Mvc;

[Route("auth")]
[ApiController]
public class AuthController : Controller
{
    private readonly IAuthService _authService;

    // Constructor al inicio
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpGet("login")] // Se asegura de que la vista de login se cargue correctamente
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromForm] string email, [FromForm] string password)
    {

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            return BadRequest(new
            {
                title = "One or more validation errors occurred.",
                status = 400,
                errors = new
                {
                    email = string.IsNullOrEmpty(email) ? new[] { "The email field is required." } : null,
                    password = string.IsNullOrEmpty(password) ? new[] { "The password field is required." } : null
                }
            });
        }

        bool isAuthenticated = await _authService.LoginAsync(email, password);

        if (!isAuthenticated)
        {
            ViewBag.Error = "Credenciales incorrectas.";
            return View("Login");
        }
        return Redirect("~/Home/Index");  // Redirección relativa
    }

    [HttpGet("register")] // Se asegura de que la vista de login se cargue correctamente
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromForm] string name, [FromForm] string email, [FromForm] string password, [FromForm] string imagenPerfilUrl)
    {
        if (string.IsNullOrEmpty(imagenPerfilUrl))
            imagenPerfilUrl = "/images/default-profile.png";

        bool isRegistered = await _authService.RegisterAsync(name, email, password, imagenPerfilUrl);

        if (!isRegistered)
        {
            ViewBag.Error = "El correo ya está registrado.";
            return RedirectToAction("Login", "Auth");
        }

        // Guardamos el mensaje en TempData
        TempData["SuccessMessage"] = "Usuario creado correctamente. Ya puedes iniciar sesión.";

        return RedirectToAction("Login", "Auth");
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Redirect("~/Auth/Login");
    }
}
