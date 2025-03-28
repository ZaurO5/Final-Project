using AutoMapper;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Category;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Category.Commands.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Response>
    {
        private readonly ICategoryReadRepository _categoryReadRepository;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateCategoryHandler(ICategoryReadRepository categoryReadRepository,
                                     ICategoryWriteRepository categoryWriteRepository,
                                     IUnitOfWork unitOfWork,
                                     IMapper mapper)
        {
            _categoryReadRepository = categoryReadRepository;
            _categoryWriteRepository = categoryWriteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<Response> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryReadRepository.GetByIdAsync(request.Id);
            if (category is null)
                throw new NotFoundException("Category is not found");

            var result = await new UpdateCategoryCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            category = _mapper.Map(request, category);
            category.ModifiedAt = DateTime.UtcNow;

            _categoryWriteRepository.Update(category);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Category updated successfully"
            };
        }
    }
}
