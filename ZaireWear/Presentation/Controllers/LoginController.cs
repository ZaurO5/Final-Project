using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers;

public class LoginController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ForgotPassword()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
}
