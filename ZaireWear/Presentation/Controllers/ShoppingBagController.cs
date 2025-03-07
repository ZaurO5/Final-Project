using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers
{
    [Authorize]
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
