using Business.Wrappers;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Business.Services.EmailHandler.Models;
using Business.Services.EmailHandler.Abstract;

namespace Business.Features.Auth.Commands.AuthForget
{
    public class AuthForgetPasswordHandler : IRequestHandler<AuthForgetPasswordCommand, Response>
    {
        private readonly UserManager<Core.Entities.User> _userManager;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthForgetPasswordHandler(
            UserManager<Core.Entities.User> userManager,
            IEmailService emailService,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        public async Task<Response> Handle(AuthForgetPasswordCommand request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request?.Email))
                throw new ValidationException("Email cannot be empty");

            var user = await _userManager.FindByEmailAsync(request.Email)
                .ConfigureAwait(false);

            if (user is null)
                return new Response { Message = "If the email exists, a password reset link has been sent" };

            var token = await _userManager.GeneratePasswordResetTokenAsync(user)
                .ConfigureAwait(false);

            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is not available");

            var encodedToken = Uri.EscapeDataString(token);
            var resetUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/Auth/ResetPassword?token={encodedToken}&email={user.Email}";

            var message = new Message(
                to: new List<string> { user.Email },
                subject: "Password Reset Request",
                content: $"To reset your password, please click the following link: <a href=\"{resetUrl}\">Reset Password</a>");

            await _emailService.SendMessageAsync(message, cancellationToken)
                .ConfigureAwait(false);

            return new Response { Message = "If the email exists, a password reset link has been sent" };
        }
    }
}
