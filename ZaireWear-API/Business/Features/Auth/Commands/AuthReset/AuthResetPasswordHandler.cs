using Business.Wrappers;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Auth.Commands.AuthReset
{
    public class AuthResetPasswordHandler : IRequestHandler<AuthResetPasswordCommand, Response>
    {
        private readonly UserManager<Core.Entities.User> _userManager;

        public AuthResetPasswordHandler(UserManager<Core.Entities.User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<Response> Handle(AuthResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new ValidationException("User not found");

            var decodedToken = Uri.UnescapeDataString(request.Token);

            var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.NewPassword);
            if (!result.Succeeded)
                throw new ValidationException(result.Errors.Select(x => x.Description));

            return new Response { Message = "Password reset successfully" };
        }
    }
}
