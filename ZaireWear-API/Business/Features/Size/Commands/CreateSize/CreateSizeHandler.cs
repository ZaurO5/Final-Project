using AutoMapper;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Size;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Size.Commands.CreateSize
{
    public class CreateSizeHandler : IRequestHandler<CreateSizeCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISizeWriteRepository _sizeWriteRepository;
        private readonly ISizeReadRepository _sizeReadRepository;
        private readonly IMapper _mapper;

        public CreateSizeHandler(IUnitOfWork unitOfWork,
                                 ISizeWriteRepository sizeWriteRepository,
                                 ISizeReadRepository sizeReadRepository,
                                 IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _sizeWriteRepository = sizeWriteRepository;
            _sizeReadRepository = sizeReadRepository;
            _mapper = mapper;
        }

        public async Task<Response> Handle(CreateSizeCommand request, CancellationToken cancellationToken)
        {
            var result = await new CreateSizeCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var size = await _sizeReadRepository.GetByNameAsync(request.Name);
            if (size is not null)
                throw new ValidationException("The size with this name already exists");

            size = _mapper.Map<Core.Entities.Size>(request);
            size.CreatedAt = DateTime.UtcNow;

            await _sizeWriteRepository.CreateAsync(size);
            await _unitOfWork.CommitAsync();

            return new Response
            {
                Message = "Size created successfully"
            };
        }
    }
}