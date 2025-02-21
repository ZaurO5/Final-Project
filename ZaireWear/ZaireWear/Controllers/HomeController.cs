using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
