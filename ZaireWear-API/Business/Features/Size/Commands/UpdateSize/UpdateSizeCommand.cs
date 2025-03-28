using Business.Wrappers;
using MediatR;

namespace Business.Features.Size.Commands.UpdateSize
{
    public class UpdateSizeCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
