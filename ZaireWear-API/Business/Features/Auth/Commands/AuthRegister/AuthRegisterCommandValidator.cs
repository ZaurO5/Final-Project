using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.Auth.Commands.AuthRegister
{
    public class AuthRegisterCommandValidator : AbstractValidator<AuthRegisterCommand>
    {
        public AuthRegisterCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email must be correct");

            RuleFor(x => x.Password.Length)
                .GreaterThanOrEqualTo(8)
                .WithMessage("Password must be at least 8 characters");

            RuleFor(x => x.Password)
                .Equal(x => x.ConfirmPassword)
                .WithMessage("Passwords is not the same");
        }
    }
}