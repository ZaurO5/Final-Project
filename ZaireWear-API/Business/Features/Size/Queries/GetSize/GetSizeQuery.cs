using Business.Features.Size.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Size.Queries.GetSize
{
    public class GetSizeQuery : IRequest<Response<SizeDto>>
    {
        public int Id { get; set; }
    }
}