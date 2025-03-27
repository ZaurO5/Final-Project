using Business.Wrappers;
using MediatR;

namespace Business.Features.Auth.Commands.AuthReset
{
    public class AuthResetPasswordCommand : IRequest<Response>
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }

}
