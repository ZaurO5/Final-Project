using FluentValidation;

namespace Business.Features.Size.Commands.CreateSize
{
    public class CreateSizeCommandValidator : AbstractValidator<CreateSizeCommand>
    {
        public CreateSizeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(5).WithMessage("Name must not exceed 5 characters.");
        }
    }
}