using Business.Wrappers;
using Core.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.AddRoleToUser;

public class AddRoleToUserHandler : IRequestHandler<AddRoleToUserCommand, Response>
{
	private readonly UserManager<Core.Entities.User> _userManager;
	private readonly RoleManager<IdentityRole> _roleManager;

	public AddRoleToUserHandler(UserManager<Core.Entities.User> userManager,
								RoleManager<IdentityRole> roleManager)
	{
		_userManager = userManager;
		_roleManager = roleManager;
	}

	public async Task<Response> Handle(AddRoleToUserCommand request, CancellationToken cancellationToken)
	{
        var result = await new AddRoleToUserCommandValidator().ValidateAsync(request);
        if (!result.IsValid)
            throw new ValidationException(result.Errors);

        var user = await _userManager.FindByIdAsync(request.UserId);
		if (user is null)
			throw new NotFoundException("User is not found");

		var role = await _roleManager.FindByIdAsync(request.RoleId);
		if (role is null)
			throw new NotFoundException("Role is not found");

		var isAlreadyExist = await _userManager.IsInRoleAsync(user, role.Name);
		if (isAlreadyExist)
			throw new ValidationException("This user already have this role");

		var addToRoleResult = await _userManager.AddToRoleAsync(user, role.Name);
		if (!addToRoleResult.Succeeded)
			throw new ValidationException(addToRoleResult.Errors.Select(x => x.Description));

		return new Response()
		{
			Message = "Role added to user successfully"
		};
	}
}
