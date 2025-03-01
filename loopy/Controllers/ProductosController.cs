using Microsoft.AspNetCore.Mvc;

namespace loopy.Controllers
{
    public class ProductosController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
