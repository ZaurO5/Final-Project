using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Category;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Category.Commands.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICategoryWriteRepository _categoryWriteRepository;
        private readonly ICategoryReadRepository _categoryReadRepository;

        public DeleteCategoryHandler(IUnitOfWork unitOfWork,
                                     ICategoryWriteRepository categoryWriteRepository,
                                     ICategoryReadRepository categoryReadRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryWriteRepository = categoryWriteRepository;
            _categoryReadRepository = categoryReadRepository;
        }

        public async Task<Response> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryReadRepository.GetByIdAsync(request.Id);
            if (category is null)
                throw new NotFoundException("Category is not found");

            _categoryWriteRepository.Delete(category);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Category deleted successfully"
            };
        }
    }
}
