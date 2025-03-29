using FluentValidation;

namespace Business.Features.Color.Commands.CreateColor
{
    public class CreateColorCommandValidator : AbstractValidator<CreateColorCommand>
    {
        public CreateColorCommandValidator()
        {
            RuleFor(x => x.Name)
               .NotEmpty().WithMessage("Name is required.")
               .MaximumLength(20).WithMessage("Name must not exceed 20 characters.");
            RuleFor(x => x.HexCode)
                .NotEmpty().WithMessage("HexCode is required.")
                .NotEmpty().Matches("^#[0-9A-Fa-f]{6}$");
        }
    }

}
