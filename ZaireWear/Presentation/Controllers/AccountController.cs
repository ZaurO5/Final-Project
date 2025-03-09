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
            {
                TempData["Success"] = "Registration successful! Please check your email.";
                return RedirectToAction("Login");
            }
            TempData["Error"] = "Registration failed. Please correct the errors.";
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ConfirmEmail(string email, string token)
        {
            if (await _accountService.ConfirmEmail(email, token))
            {
                TempData["Success"] = "Email confirmed successfully!";
                return RedirectToAction(nameof(Login));
            }
            TempData["Error"] = "Email confirmation failed.";
            return RedirectToAction(nameof(Login));
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
            TempData["Error"] = "Invalid login attempt.";
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
            if (await _accountService.ForgetPasswordAsync(model))
            {
                TempData["Success"] = "Password reset instructions sent to your email.";
                return RedirectToAction("Login");
            }
            TempData["Error"] = "Error processing your request.";
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email) => View(new ResetPasswordVM { Token = token, Email = email });

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountService.ResetPassword(model);
            if (result.Succeeded)
            {
                TempData["Success"] = "Password reset successfully!";
                return RedirectToAction("Login");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }
    }
}
