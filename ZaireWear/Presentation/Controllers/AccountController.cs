using Business.Services.Abstract;
using Business.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService) => _accountService = accountService;

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(AccountRegisterVM model)
        {
            if (await _accountService.RegisterAsync(model))
                return RedirectToAction("Login");
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(AccountLoginVM model)
        {
            var (isSucceeded, returnUrl) = await _accountService.LoginAsync(model);
            if (isSucceeded)
            {
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult ForgetPassword() => View();

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
        {
            return await _accountService.ForgetPasswordAsync(model)
                ? RedirectToAction("Login")
                : View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email) => View(new ResetPasswordVM { Token = token, Email = email });

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            return await _accountService.ResetPassword(model)
                ? RedirectToAction("Login")
                : View(model);
        }
    }
}
