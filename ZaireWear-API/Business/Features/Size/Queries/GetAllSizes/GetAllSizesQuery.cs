using Business.Features.Size.Dtos;
using Business.Wrappers;
using MediatR;


namespace Business.Features.Size.Queries.GetAllSizes
{
    public class GetAllSizesQuery : IRequest<Response<List<SizeDto>>>
    {
    }
}
