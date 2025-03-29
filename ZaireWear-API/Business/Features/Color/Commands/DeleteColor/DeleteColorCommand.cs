using Business.Wrappers;
using MediatR;

namespace Business.Features.Color.Commands.DeleteColor
{
    public class DeleteColorCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }

}
