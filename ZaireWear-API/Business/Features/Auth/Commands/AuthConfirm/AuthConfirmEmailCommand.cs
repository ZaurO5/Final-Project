using Business.Wrappers;
using MediatR;

namespace Business.Features.Auth.Commands.AuthConfirm
{
    public class AuthConfirmEmailCommand : IRequest<Response>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }

}
