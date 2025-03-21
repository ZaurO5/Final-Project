using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers;

[Authorize]
public class CheckoutController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
