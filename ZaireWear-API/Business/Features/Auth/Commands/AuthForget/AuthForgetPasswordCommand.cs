using Business.Wrappers;
using MediatR;

namespace Business.Features.Auth.Commands.AuthForget
{
    public class AuthForgetPasswordCommand : IRequest<Response>
    {
        public string Email { get; set; }
    }
}
