using Business.Wrappers;
using MediatR;

namespace Business.Features.Size.Commands.CreateSize
{
    public class CreateSizeCommand : IRequest<Response>
    {
        public string Name { get; set; }
    }
}