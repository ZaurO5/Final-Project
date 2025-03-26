using Business.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Features.UserRole.Commands.AddRoleToUser;

public class AddRoleToUserCommand : IRequest<Response>
{
	public string UserId { get; set; }
	public string RoleId { get; set; }
}
