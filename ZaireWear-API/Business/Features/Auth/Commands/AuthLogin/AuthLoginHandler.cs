using AutoMapper;
using Business.Features.Auth.Dtos;
using Business.Wrappers;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Business.Features.Auth.Commands.AuthLogin
{
    public class AuthLoginHandler : IRequestHandler<AuthLoginCommand, Response<ResponseTokenDto>>
    {
        private readonly UserManager<Core.Entities.User> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthLoginHandler(UserManager<Core.Entities.User> userManager,
                                IMapper mapper,
                                IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<Response<ResponseTokenDto>> Handle(AuthLoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
                throw new UnauthorizedException("Email or password are incorrect");

            var isSuccededCheck = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!isSuccededCheck)
                throw new UnauthorizedException("Email or password are incorrect");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Email)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role));


            var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

            var token = new JwtSecurityToken(
                claims: claims,
                issuer: _configuration.GetSection("JWT:Issuer").Value,
                audience: _configuration.GetSection("JWT:Audience").Value,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256)
                );

            return new Response<ResponseTokenDto>
            {
                Data = new ResponseTokenDto
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token)
                }
            };
        }
    }
}