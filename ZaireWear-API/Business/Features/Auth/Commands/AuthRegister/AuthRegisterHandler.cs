using AutoMapper;
using Business.Services.EmailHandler.Abstract;
using Business.Services.EmailHandler.Models;
using Business.Wrappers;
using Core.Constants.Enums;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Auth.Commands.AuthRegister
{
    public class AuthRegisterHandler : IRequestHandler<AuthRegisterCommand, Response>
    {
        private readonly UserManager<Core.Entities.User> _userManager;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthRegisterHandler(UserManager<Core.Entities.User> userManager,
                                   IMapper mapper,
                                   IEmailService emailService,
                                   IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _mapper = mapper;
            _emailService = emailService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Response> Handle(AuthRegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await new AuthRegisterCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
                throw new ValidationException("Email already exists.");

            user = _mapper.Map<Core.Entities.User>(request);
            user.EmailConfirmed = false;

            var registerResult = await _userManager.CreateAsync(user, request.Password);
            if (!registerResult.Succeeded)
                throw new ValidationException(registerResult.Errors.Select(x => x.Description));

            var addToRoleResult = await _userManager.AddToRoleAsync(user, UserRoles.Client.ToString());
            if (!addToRoleResult.Succeeded)
                throw new ValidationException(addToRoleResult.Errors.Select(x => x.Description));

            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var httpContext = _httpContextAccessor.HttpContext
                ?? throw new InvalidOperationException("HTTP context is not available");

            var encodedToken = Uri.EscapeDataString(token);
            var confirmEmailUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/Auth/ConfirmEmail?token={encodedToken}&email={user.Email}";

            var message = new Message(
                to: new List<string> { user.Email },
                subject: "Email Confirmation",
                content: $"To confirm your email address, please click the following link: <a href=\"{confirmEmailUrl}\">Confirm Email</a>");

            await _emailService.SendMessageAsync(message, cancellationToken);

            return new Response
            {
                Message = "User registered successfully. A confirmation email has been sent."
            };
        }
    }
}