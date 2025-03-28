using Business.Features.Category.Dtos;
using Business.Wrappers;
using MediatR;

namespace Business.Features.Category.Queries.GetAllCategories
{
    public class GetAllCategoriesQuery : IRequest<Response<List<CategoryDto>>>
    {
    }
}
