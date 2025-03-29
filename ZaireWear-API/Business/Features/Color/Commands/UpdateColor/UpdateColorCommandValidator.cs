using FluentValidation;

namespace Business.Features.Color.Commands.UpdateColor
{
    public class UpdateColorCommandValidator : AbstractValidator<UpdateColorCommand>
    {
        public UpdateColorCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");

            RuleFor(x => x.HexCode)
                .NotEmpty().WithMessage("HexCode is required.")
                .Matches("^#[0-9A-Fa-f]{6}$").WithMessage("HexCode must be in the format #RRGGBB.");
        }
    }
}
