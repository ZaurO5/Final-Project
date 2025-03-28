using Business.Wrappers;
using MediatR;

namespace Business.Features.Size.Commands.DeleteSize
{
    public class DeleteSizeCommand : IRequest<Response>
    {
        public int Id { get; set; }
    }
}
