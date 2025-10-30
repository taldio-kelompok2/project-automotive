using AutoMapper;
using AutomotiveApp.Application.Mapper.Resolver;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.Application.Mapper
{
    public class CourseCategoryProfile : Profile
    {
        public CourseCategoryProfile()
        {
            CreateMap<CourseCategory, CourseCategoryQueryDto>()
            .ForMember(dest => dest.ImageUrl, opt =>
                opt.MapFrom<ImageUrlResolver<CourseCategory, CourseCategoryQueryDto>>())
            .ForMember(dest => dest.HeroImageUrl, opt =>
                opt.MapFrom<HeroImageUrlResolver<CourseCategory, CourseCategoryQueryDto>>());

            CreateMap<CourseCategoryQueryDto, CourseCategory>()
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

            CreateMap<CourseCategoryCommandDto, CourseCategory>()
            .ForMember(dest => dest.Courses, opt => opt.Ignore());

            CreateMap<CourseCategoryEditDto, CourseCategory>()
            .ForMember(dest => dest.Description,
                opt => opt.Condition((src, dest, srcMember) => srcMember != null))
            .ForMember(dest => dest.Name,
                opt => opt.Condition((src, dest, srcMember) => srcMember != null))
            .ForMember(dest => dest.ImageFileName,
                opt => opt.Condition((src, dest, srcMember) => srcMember != null))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}