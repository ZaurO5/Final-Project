using Business.Features.Auth.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Auth.Commands.AuthLogin
{
    public class AuthLoginCommand : IRequest<Response<ResponseTokenDto>>
    {
	public string Email { get; set; }
    public string Password { get; set; }
    }
}