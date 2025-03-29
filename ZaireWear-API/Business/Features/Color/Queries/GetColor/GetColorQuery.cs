using Business.Features.Color.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Color.Queries.GetColor
{
    public class GetColorQuery : IRequest<Response<ColorDto>>
    {
        public int Id { get; set; }
    }

}
