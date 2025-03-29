using Business.Wrappers;
using MediatR;

namespace Business.Features.Color.Commands.UpdateColor
{
    public class UpdateColorCommand : IRequest<Response>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }
    }

}
