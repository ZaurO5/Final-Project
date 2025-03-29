using AutoMapper;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Color;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Color.Commands.CreateColor
{
    public class CreateColorHandler : IRequestHandler<CreateColorCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColorWriteRepository _colorWriteRepository;
        private readonly IColorReadRepository _colorReadRepository;
        private readonly IMapper _mapper;

        public CreateColorHandler(IUnitOfWork unitOfWork,
                                  IColorWriteRepository colorWriteRepository,
                                  IColorReadRepository colorReadRepository,
                                  IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _colorWriteRepository = colorWriteRepository;
            _colorReadRepository = colorReadRepository;
            _mapper = mapper;
        }

        public async Task<Response> Handle(CreateColorCommand request, CancellationToken cancellationToken)
        {
            var result = await new CreateColorCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            var color = await _colorReadRepository.GetByNameAsync(request.Name);
            if (color is not null)
                throw new ValidationException("The color with this name already exists");

            color = _mapper.Map<Core.Entities.Color>(request);
            color.CreatedAt = DateTime.UtcNow;

            await _colorWriteRepository.CreateAsync(color);
            await _unitOfWork.CommitAsync();

            return new Response
            { 
                Message = "Color created successfully" 
            };
        }
    }

}
