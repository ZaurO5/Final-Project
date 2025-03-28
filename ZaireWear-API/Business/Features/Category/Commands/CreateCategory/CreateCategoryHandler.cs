using AutoMapper;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Category;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Category.Commands.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly IMapper _mapper;

        public CreateCategoryHandler(IUnitOfWork unitOfWork,
                                     ICategoryWriteRepository categoryWriteRepository,
                                     ICategoryReadRepository categoryReadRepository,
                                     IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
            _mapper = mapper;
        }

        public async Task<Response> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var result = await new CreateCategoryCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var category = await _categoryReadRepository.GetByNameAsync(request.Name);
            if (category is not null)
                throw new ValidationException("The category with this name is already exist");

            category = _mapper.Map<Core.Entities.Category>(request);
            category.CreatedAt = DateTime.UtcNow;

            await _categoryWriteRepository.CreateAsync(category);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Category created successfully"
            };
        }
    }
}
