using AutoMapper;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Size;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Size.Commands.UpdateSize
{
    public class UpdateSizeHandler : IRequestHandler<UpdateSizeCommand, Response>
    {
        private readonly ISizeReadRepository _sizeReadRepository;
        private readonly ISizeWriteRepository _sizeWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateSizeHandler(ISizeReadRepository sizeReadRepository,
                                 ISizeWriteRepository sizeWriteRepository,
                                 IUnitOfWork unitOfWork,
                                 IMapper mapper)
        {
            _sizeReadRepository = sizeReadRepository;
            _sizeWriteRepository = sizeWriteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(UpdateSizeCommand request, CancellationToken cancellationToken)
        {
            var size = await _sizeReadRepository.GetByIdAsync(request.Id);
            if (size is null)
                throw new NotFoundException("Size is not found");

            var result = await new UpdateSizeCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            size = _mapper.Map(request, size);
            size.ModifiedAt = DateTime.UtcNow;

            _sizeWriteRepository.Update(size);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Size updated successfully"
            };
        }
    }
}