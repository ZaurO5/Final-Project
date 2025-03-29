using AutoMapper;
using Business.Features.Color.Commands.CreateColor;
using Business.Features.Color.Commands.UpdateColor;
using Business.Features.Color.Dtos;
using Core.Entities;

namespace Business.MappingProfiles
{
    public class ColorMappingProfile : Profile
    {
        public ColorMappingProfile()
        {
            CreateMap<CreateColorCommand, Color>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore());

            CreateMap<Color, ColorDto>();

            CreateMap<UpdateColorCommand, Color>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore());
        }
    }

}
