using AutoMapper;
using Business.Features.Category.Dtos;
using Business.Wrappers;
using Data.Repositories.Category;
using MediatR;

namespace Business.Features.Category.Queries.GetAllCategories
{
    public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, Response<List<CategoryDto>>>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IMapper _mapper;

        public GetAllCategoriesHandler(ICategoryReadRepository categoryReadRepository,
                                      IMapper mapper)
        {
            _categoryReadRepository = categoryReadRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            return new Response<List<CategoryDto>>()
            {
                Data = _mapper.Map<List<CategoryDto>>(await _categoryReadRepository.GetAllAsync())
            };
        }
    }
}
