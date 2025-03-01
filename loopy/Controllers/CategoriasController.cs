using Microsoft.AspNetCore.Mvc;

namespace loopy.Controllers
{
    public class CategoriasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
