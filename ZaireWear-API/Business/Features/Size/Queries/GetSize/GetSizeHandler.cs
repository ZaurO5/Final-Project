using AutoMapper;
using Business.Features.Size.Dtos;
using Business.Wrappers;
using Core.Exceptions;
using Data.Repositories.Size;
using MediatR;

namespace Business.Features.Size.Queries.GetSize
{
    public class GetSizeHandler : IRequestHandler<GetSizeQuery, Response<SizeDto>>
    {
        private readonly ISizeReadRepository _sizeReadRepository;
        private readonly IMapper _mapper;

        public GetSizeHandler(ISizeReadRepository sizeReadRepository,
                              IMapper mapper)
        {
            _sizeReadRepository = sizeReadRepository;
            _mapper = mapper;
        }

        public async Task<Response<SizeDto>> Handle(GetSizeQuery request, CancellationToken cancellationToken)
        {
            var size = await _sizeReadRepository.GetByIdAsync(request.Id);
            if (size is null)
                throw new NotFoundException("Size is not found");

            return new Response<SizeDto>()
            {
                Data = _mapper.Map<SizeDto>(size)
            };
        }
    }
}
