using Microsoft.AspNetCore.Mvc;

namespace loopy.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
