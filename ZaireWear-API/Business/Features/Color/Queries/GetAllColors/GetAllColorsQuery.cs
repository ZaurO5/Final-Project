using Business.Features.Color.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Color.Queries.GetAllColors
{
    public class GetAllColorsQuery : IRequest<Response<List<ColorDto>>>
    {
    }

}
