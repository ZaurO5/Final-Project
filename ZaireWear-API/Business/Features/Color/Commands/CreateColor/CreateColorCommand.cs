using Business.Wrappers;
using MediatR;

namespace Business.Features.Color.Commands.CreateColor
{
    public class CreateColorCommand : IRequest<Response>
    {
        public string Name { get; set; }
        public string HexCode { get; set; }
    }

}
