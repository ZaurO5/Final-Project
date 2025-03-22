using Business.Services.Abstract;
using Business.Utilities.EmailHandler.Abstract;
using Core.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using Business.Utilities.EmailHandler.Models;
using Business.ViewModels.Account;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;

namespace Business.Services.Concrete;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IEmailService _emailService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IActionContextAccessor _actionContextAccessor;

    public AccountService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IEmailService emailService,
        IHttpContextAccessor httpContextAccessor,
        IActionContextAccessor actionContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _httpContextAccessor = httpContextAccessor;
        _actionContextAccessor = actionContextAccessor;
    }

    public async Task<bool> RegisterAsync(AccountRegisterVM model)
    {
        if (!_actionContextAccessor.ActionContext.ModelState.IsValid) return false;

        var user = new User
        {
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, error.Description);
            }
            return false;
        }

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var urlHelper = new UrlHelper(_actionContextAccessor.ActionContext);
        var url = urlHelper.Action("ConfirmEmail", "Account", new { token, email = user.Email }, _httpContextAccessor.HttpContext.Request.Scheme);

        _emailService.SendMessage(new Message(new List<string> { user.Email }, "Email Confirmation", url));
        return true;
    }

    public async Task<bool> ConfirmEmail(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
        {
            _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "User not found.");
            return false;
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<(bool IsSucceeded, string? returnUrl)> LoginAsync(AccountLoginVM model)
    {
        if (!_actionContextAccessor.ActionContext.ModelState.IsValid) return (false, null);

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Wrong Email or Password");
            return (false, null);
        }

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        if (!result.Succeeded)
        {
            _actionContextAccessor.ActionContext.ModelState.AddModelError(string.Empty, "Wrong Email or Password");
            return (false, null);
        }

        return (true, "/Home/Index");
    }

    public async Task LogoutAsync() => await _signInManager.SignOutAsync();

    public async Task<bool> ForgetPasswordAsync(ForgetPasswordVM model)
    {
        if (!_actionContextAccessor.ActionContext.ModelState.IsValid) return false;

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var urlHelper = new UrlHelper(_actionContextAccessor.ActionContext);
        var url = urlHelper.Action("ResetPassword", "Account", new { token, email = user.Email }, _httpContextAccessor.HttpContext.Request.Scheme);

        _emailService.SendMessage(new Message(new List<string> { user.Email }, "Reset Password", url));
        return true;
    }

    public async Task<IdentityResult> ResetPassword(ResetPasswordVM model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError
            {
                Description = "User not found"
            });
        }

        return await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
    }
}
