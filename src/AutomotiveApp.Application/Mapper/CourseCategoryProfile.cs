using AutoMapper;
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
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.ToString()));

            CreateMap<CourseCategoryQueryDto, CourseCategory>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => Enum.Parse<CarCategory>(src.Name)))
            .ForMember(dest => dest.Courses, opt => opt.Ignore());
        }
    }
}