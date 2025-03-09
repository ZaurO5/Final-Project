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

namespace Business.Services.Concrete
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailService _emailService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ModelStateDictionary _modelState;

        public AccountService(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            IEmailService emailService,
            IActionContextAccessor actionContextAccessor,
            IUrlHelperFactory urlHelperFactory,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _urlHelperFactory = urlHelperFactory;
            _httpContextAccessor = httpContextAccessor;
            _modelState = actionContextAccessor.ActionContext.ModelState;
        }

        public async Task<bool> RegisterAsync(AccountRegisterVM model)
        {
            if (!_modelState.IsValid) return false;

            try
            {
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
                        _modelState.AddModelError(string.Empty, error.Description);
                    return false;
                }

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(
                    _httpContextAccessor.HttpContext,
                    _httpContextAccessor.HttpContext.GetRouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                ));
                var url = urlHelper.Action("ConfirmEmail", "Account", new { token, email = user.Email }, "https");
                _emailService.SendMessage(new Message(new List<string> { user.Email }, "Email Confirmation", url));

                return true;
            }
            catch (Exception ex)
            {
                _modelState.AddModelError(string.Empty, "Error during registration. Please try again.");
                return false;
            }
        }
        public async Task<bool> ConfirmEmail(string email, string token)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _modelState.AddModelError(string.Empty, "User not found.");
                    return false;
                }
                return (await _userManager.ConfirmEmailAsync(user, token)).Succeeded;
            }
            catch (Exception)
            {
                _modelState.AddModelError(string.Empty, "Error confirming email.");
                return false;
            }
        }

        public async Task<(bool IsSucceeded, string? returnUrl)> LoginAsync(AccountLoginVM model)
        {
            if (!_modelState.IsValid) return (false, null);

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is null)
                {
                    _modelState.AddModelError(string.Empty, "Wrong Email or Password");
                    return (false, null);
                }

                var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                if (!result.Succeeded)
                {
                    _modelState.AddModelError(string.Empty, "Wrong Email or Password");
                    return (false, null);
                }

                return (true, "/Home/Index");
            }
            catch (Exception)
            {
                _modelState.AddModelError(string.Empty, "Login error. Please try again.");
                return (false, null);
            }
        }

        public async Task LogoutAsync() => await _signInManager.SignOutAsync();

        public async Task<bool> ForgetPasswordAsync(ForgetPasswordVM model)
        {
            if (!_modelState.IsValid) return false;

            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null) return false;

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var urlHelper = _urlHelperFactory.GetUrlHelper(new ActionContext(
                    _httpContextAccessor.HttpContext,
                    _httpContextAccessor.HttpContext.GetRouteData(),
                    new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
                ));
                var url = urlHelper.Action("ResetPassword", "Account", new { token, email = user.Email }, "https");
                _emailService.SendMessage(new Message(new List<string> { user.Email }, "Reset Password", url));

                return true;
            }
            catch (Exception)
            {
                _modelState.AddModelError(string.Empty, "Error processing password reset.");
                return false;
            }
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordVM model)
        {
            try
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
            catch (Exception)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Description = "Error resetting password"
                });
            }
        }
    }
}
