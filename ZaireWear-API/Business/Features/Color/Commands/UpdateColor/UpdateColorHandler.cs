using AutoMapper;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Color;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Color.Commands.UpdateColor
{
    public class UpdateColorHandler : IRequestHandler<UpdateColorCommand, Response>
    {
        private readonly IColorReadRepository _colorReadRepository;
        private readonly IColorWriteRepository _colorWriteRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public UpdateColorHandler(IColorReadRepository colorReadRepository,
                                  IColorWriteRepository colorWriteRepository,
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper)
        {
            _colorReadRepository = colorReadRepository;
            _colorWriteRepository = colorWriteRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
        {
            var color = await _colorReadRepository.GetByIdAsync(request.Id);
            if (color is null)
                throw new NotFoundException("Color is not found");

            var result = await new UpdateColorCommandValidator().ValidateAsync(request);
            if (!result.IsValid)
                throw new ValidationException(result.Errors);

            color = _mapper.Map(request, color);
            color.ModifiedAt = DateTime.UtcNow;

            _colorWriteRepository.Update(color);
            await _unitOfWork.CommitAsync();

            return new Response
            { 
                Message = "Color updated successfully" 
            };
        }
    }

}
