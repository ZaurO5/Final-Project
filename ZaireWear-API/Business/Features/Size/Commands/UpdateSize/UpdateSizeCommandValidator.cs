using FluentValidation;

namespace Business.Features.Size.Commands.UpdateSize
{
    public class UpdateSizeCommandValidator : AbstractValidator<UpdateSizeCommand>
    {
        public UpdateSizeCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");
        }
    }
}