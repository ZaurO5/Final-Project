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
    private readonly LinkGenerator _linkGenerator;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IEmailService emailService,
        LinkGenerator linkGenerator,
        IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _emailService = emailService;
        _linkGenerator = linkGenerator;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> RegisterAsync(AccountRegisterVM model)
    {
        var user = new User
        {
            Email = model.Email,
            UserName = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded) return false;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
        var url = _linkGenerator.GetUriByAction(
            "ConfirmEmail",
            "Account",
            new { token, email = user.Email },
            scheme: _httpContextAccessor.HttpContext.Request.Scheme,
            host: _httpContextAccessor.HttpContext.Request.Host);

        await _emailService.SendMessageAsync(new Message(
            new List<string> { user.Email },
            "Confirm Your Email",
            $"Please confirm your email by clicking this link: {url}"));

        return true;
    }

    public async Task<bool> ConfirmEmail(string email, string token)
    {
        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) return false;

        var result = await _userManager.ConfirmEmailAsync(user, token);
        return result.Succeeded;
    }

    public async Task<(bool IsSucceeded, string? returnUrl)> LoginAsync(AccountLoginVM model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return (false, null);

        var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
        return result.Succeeded ? (true, model.ReturnUrl ?? "/Home/Index") : (false, null);
    }

    public Task LogoutAsync() => _signInManager.SignOutAsync();

    public async Task<bool> ForgetPasswordAsync(ForgetPasswordVM model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null) return false;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var url = _linkGenerator.GetUriByAction(
            "ResetPassword",
            "Account",
            new { token, email = user.Email },
            scheme: _httpContextAccessor.HttpContext.Request.Scheme,
            host: _httpContextAccessor.HttpContext.Request.Host);

        await _emailService.SendMessageAsync(new Message(
            new List<string> { user.Email },
            "Password Reset Request",
            $"Reset your password by clicking this link: {url}"));

        return true;
    }

    public async Task<IdentityResult> ResetPassword(ResetPasswordVM model)
    {
        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return IdentityResult.Failed(new IdentityError { Description = "User not found" });
        }

        return await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
    }
}