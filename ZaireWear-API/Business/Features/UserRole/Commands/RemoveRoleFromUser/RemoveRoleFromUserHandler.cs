using Business.Features.UserRole.Commands.AddRoleToUser;
using Business.Wrappers;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.RemoveRoleFromUser;

public class RemoveRoleFromUserHandler : IRequestHandler<RemoveRoleFromUserCommand, Response>
{
	private readonly UserManager<Core.Entities.User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public RemoveRoleFromUserHandler(UserManager<Core.Entities.User> userManager,
									 RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task<Response> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
	{
        var result = await new RemoveRoleFromUserCommandValidator().ValidateAsync(request);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var user = await _userManager.FindByIdAsync(request.UserId);
		if (user is null)
			throw new NotFoundException("User is not found");

		var role = await _roleManager.FindByIdAsync(request.RoleId);
		if (role is null)
			throw new NotFoundException("Role is not found");

		var isInRole = await _userManager.IsInRoleAsync(user, role.Name);
		if (!isInRole)
			throw new ValidationException("User doesn't have this role");

		var removeResult = await _userManager.RemoveFromRoleAsync(user, role.Name);
		if (!removeResult.Succeeded)
			throw new ValidationException(removeResult.Errors.Select(x => x.Description));

		return new Response()
		{
			Message = "Role removed from this user successfully"
		};
	}
}
