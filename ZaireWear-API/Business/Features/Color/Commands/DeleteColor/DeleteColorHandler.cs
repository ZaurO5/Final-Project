using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Color;
using Data.UnitOfWork;
using MediatR;

namespace Business.Features.Color.Commands.DeleteColor
{
    public class DeleteColorHandler : IRequestHandler<DeleteColorCommand, Response>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IColorWriteRepository _colorWriteRepository;
        private readonly IColorReadRepository _colorReadRepository;

        public DeleteColorHandler(IUnitOfWork unitOfWork,
                                  IColorWriteRepository colorWriteRepository,
                                  IColorReadRepository colorReadRepository)
        {
            _unitOfWork = unitOfWork;
            _colorWriteRepository = colorWriteRepository;
            _colorReadRepository = colorReadRepository;
        }

        public async Task<Response> Handle(DeleteColorCommand request, CancellationToken cancellationToken)
        {
            var color = await _colorReadRepository.GetByIdAsync(request.Id);
            if (color is null)
                throw new NotFoundException("Color is not found");

            _colorWriteRepository.Delete(color);
            await _unitOfWork.CommitAsync();

            return new Response
            { 
                Message = "Color deleted successfully" 
            };
        }
    }

}
