using AutoMapper;
using Business.Features.Color.Dtos;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Color;
using MediatR;

namespace Business.Features.Color.Queries.GetColor
{
    public class GetColorHandler : IRequestHandler<GetColorQuery, Response<ColorDto>>
    {
        private readonly IColorReadRepository _colorReadRepository;
        private readonly IMapper _mapper;

        public GetColorHandler(IColorReadRepository colorReadRepository, IMapper mapper)
        {
            _colorReadRepository = colorReadRepository;
            _mapper = mapper;
        }

        public async Task<Response<ColorDto>> Handle(GetColorQuery request, CancellationToken cancellationToken)
        {
            var color = await _colorReadRepository.GetByIdAsync(request.Id);
            if (color is null)
                throw new NotFoundException("Color is not found");

            return new Response<ColorDto>() 
            { 
                Data = _mapper.Map<ColorDto>(color)
            };
        }
    }

}
