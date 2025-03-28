using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Size;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Size.Commands.DeleteSize
{
    public class DeleteSizeHandler : IRequestHandler<DeleteSizeCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISizeWriteRepository _sizeWriteRepository;
        private readonly ISizeReadRepository _sizeReadRepository;

        public DeleteSizeHandler(IUnitOfWork unitOfWork,
                                 ISizeWriteRepository sizeWriteRepository,
                                 ISizeReadRepository sizeReadRepository)
        {
            _unitOfWork = unitOfWork;
            _sizeWriteRepository = sizeWriteRepository;
            _sizeReadRepository = sizeReadRepository;
        }

        public async Task<Response> Handle(DeleteSizeCommand request, CancellationToken cancellationToken)
        {
            var size = await _sizeReadRepository.GetByIdAsync(request.Id);
            if (size is null)
                throw new NotFoundException("Size is not found");

            _sizeWriteRepository.Delete(size);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Size deleted successfully"
            };
        }
    }
}