using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers
{
    public class ShoppingBagController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Wishlist()
        {
            return View();
        }
    }
}
