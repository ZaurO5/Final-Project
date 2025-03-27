using FluentValidation;

namespace Business.Features.Auth.Commands.AuthReset
{
    public class AuthResetPasswordCommandValidator : AbstractValidator<AuthResetPasswordCommand>
    {
        public AuthResetPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format");

            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("New password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long");

            RuleFor(x => x.NewPassword)
                .Equal(x => x.ConfirmPassword).WithMessage("Passwords do not match");
        }
    }

}
