using AutoMapper;
using Business.Features.Size.Dtos;
using Business.Wrappers;
using Data.Repositories.Size;
using MediatR;

namespace Business.Features.Size.Queries.GetAllSizes
{
    public class GetAllSizesHandler : IRequestHandler<GetAllSizesQuery, Response<List<SizeDto>>>
    {
        private readonly ISizeReadRepository _sizeReadRepository;
        private readonly IMapper _mapper;

        public GetAllSizesHandler(ISizeReadRepository sizeReadRepository,
                                      IMapper mapper)
        {
            _sizeReadRepository = sizeReadRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<SizeDto>>> Handle(GetAllSizesQuery request, CancellationToken cancellationToken)
        {
            return new Response<List<SizeDto>>()
            {
                Data = _mapper.Map<List<SizeDto>>(await _sizeReadRepository.GetAllAsync())
            };
        }
    }
}
