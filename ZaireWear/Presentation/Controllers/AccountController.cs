using Business.Services.Abstract;
using Business.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;

namespace ZaireWear.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService) => _accountService = accountService;

    [HttpGet]
    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(AccountRegisterVM model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _accountService.RegisterAsync(model))
        {
            TempData["Success"] = "Registered successfully! Please check your email to confirm.";
            return RedirectToAction("Login");
        }

        TempData["Error"] = "Registration failed. Please try again.";
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> ConfirmEmail(string email, string token)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(token))
        {
            TempData["Error"] = "Invalid email confirmation link.";
            return RedirectToAction("Login");
        }

        if (await _accountService.ConfirmEmail(email, token))
        {
            TempData["Success"] = "Email confirmed successfully! You can now log in.";
            return RedirectToAction("Login");
        }

        TempData["Error"] = "Email confirmation failed. Please try again.";
        return RedirectToAction("Login");
    }

    [HttpGet]
    public IActionResult Login(string? returnUrl = null) => View(new AccountLoginVM { ReturnUrl = returnUrl });

    [HttpPost]
    public async Task<IActionResult> Login(AccountLoginVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var (isSucceeded, returnUrl) = await _accountService.LoginAsync(model);
        if (isSucceeded)
        {
            TempData["Success"] = "Logged in successfully!";
            return Redirect(returnUrl);
        }

        TempData["Error"] = "Login failed. Invalid email or password.";
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _accountService.LogoutAsync();
        TempData["Success"] = "Logged out successfully!";
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    public IActionResult ForgetPassword() => View();

    [HttpPost]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordVM model)
    {
        if (!ModelState.IsValid) return View(model);

        if (await _accountService.ForgetPasswordAsync(model))
        {
            TempData["Success"] = "Password reset link has been sent to your email.";
            return RedirectToAction("Login");
        }

        TempData["Error"] = "Failed to send reset link. Please check your email address.";
        return View(model);
    }

    [HttpGet]
    public IActionResult ResetPassword(string token, string email)
        => View(new ResetPasswordVM { Token = token, Email = email });

    [HttpPost]
    public async Task<IActionResult> ResetPassword(ResetPasswordVM model)
    {
        if (!ModelState.IsValid) return View(model);

        var result = await _accountService.ResetPassword(model);
        if (result.Succeeded)
        {
            TempData["Success"] = "Password has been reset successfully! Please log in.";
            return RedirectToAction("Login");
        }

        TempData["Error"] = "Password reset failed. Invalid token or email.";
        return View(model);
    }
}