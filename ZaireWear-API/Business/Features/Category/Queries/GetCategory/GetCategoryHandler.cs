using AutoMapper;
using Business.Features.Category.Dtos;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Category;
using MediatR;

namespace Business.Features.Category.Queries.GetCategory
{
    public class GetCategoryHandler : IRequestHandler<GetCategoryQuery, Response<CategoryDto>>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IMapper _mapper;

        public GetCategoryHandler(ICategoryReadRepository categoryReadRepository,
                                  IMapper mapper)
        {
            _categoryReadRepository = categoryReadRepository;
            _mapper = mapper;
        }

        public async Task<Response<CategoryDto>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryReadRepository.GetByIdAsync(request.Id);
            if (category is null)
                throw new NotFoundException("Category is not found");

            return new Response<CategoryDto>()
            {
                Data = _mapper.Map<CategoryDto>(category)
            };
        }
    }
}
