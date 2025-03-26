using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserCommandValidator : AbstractValidator<RemoveRoleFromUserCommand>
{
	public RemoveRoleFromUserCommandValidator()
	{
		RuleFor(x => x.UserId)
		  .NotEmpty()
		  .WithMessage("User is required");

		RuleFor(x => x.RoleId)
			.NotEmpty()
			.WithMessage("Role is required");
	}
}
