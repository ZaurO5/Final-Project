using Business.Wrappers;
using MediatR;

namespace Business.Features.Auth.Commands.AuthRegister
{
    public class AuthRegisterCommand : IRequest<Response>
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}