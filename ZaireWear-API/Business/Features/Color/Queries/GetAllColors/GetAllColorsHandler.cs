using AutoMapper;
using Business.Features.Color.Dtos;
using Business.Wrappers;
using Data.Repositories.Color;
using MediatR;

namespace Business.Features.Color.Queries.GetAllColors
{
    public class GetAllColorsHandler : IRequestHandler<GetAllColorsQuery, Response<List<ColorDto>>>
    {
        private readonly IColorReadRepository _colorReadRepository;
        private readonly IMapper _mapper;

        public GetAllColorsHandler(IColorReadRepository colorReadRepository, IMapper mapper)
        {
            _colorReadRepository = colorReadRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<ColorDto>>> Handle(GetAllColorsQuery request, CancellationToken cancellationToken)
        {
            return new Response<List<ColorDto>>()
            {
                Data = _mapper.Map<List<ColorDto>>(await _colorReadRepository.GetAllAsync())
            };
        }
    }

}
