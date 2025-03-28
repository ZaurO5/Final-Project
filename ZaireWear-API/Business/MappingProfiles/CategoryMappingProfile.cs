using AutoMapper;
using Business.Features.Category.Commands.CreateCategory;
using Business.Features.Category.Commands.UpdateCategory;
using Business.Features.Category.Dtos;
using Core.Entities;

namespace Business.MappingProfiles
{
    public class CategoryMappingProfile : Profile
    {
        public CategoryMappingProfile()
        {
            CreateMap<CreateCategoryCommand, Category>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore());

            CreateMap<Category, CategoryDto>();

            CreateMap<UpdateCategoryCommand, Category>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.ModifiedAt, opt => opt.Ignore());
        }
    }
}
