using AutoMapper;
using Business.Features.Size.Commands.CreateSize;
using Business.Features.Size.Commands.UpdateSize;
using Business.Features.Size.Dtos;
using Core.Entities;


namespace Business.MappingProfiles
{
    public class SizeMappingProfile : Profile
    {
        public SizeMappingProfile()
        {
            CreateMap<CreateSizeCommand, Size>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore());

            CreateMap<Size, SizeDto>();

            CreateMap<UpdateSizeCommand, Size>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore());
        }
    }
}
