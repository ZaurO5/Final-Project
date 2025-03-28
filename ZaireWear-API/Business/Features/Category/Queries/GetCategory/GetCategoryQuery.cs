using Business.Features.Category.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Category.Queries.GetCategory
{
    public class GetCategoryQuery : IRequest<Response<CategoryDto>>
    {
        public int Id { get; set; }
    }
}
