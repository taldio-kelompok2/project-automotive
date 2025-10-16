using AutoMapper;
using AutomotiveApp.Application.Mapper.Resolver;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;

namespace AutomotiveApp.Application.Mapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseQueryDto>()
            .ForMember(dest => dest.ImageUrl, opt =>
            opt.MapFrom<ImageUrlResolver<Course, CourseQueryDto>>())
            .ForMember(dest => dest.Category,
            opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.Price, opt => opt.MapFrom(src => (int)src.Price));

            CreateMap<Course, CourseQueryDetailDto>()
            .ForMember(dest => dest.Sessions,
            opt => opt.MapFrom(src => src.Sessions
            .OrderBy(s => s.Date)
            .Select(s => s.Date)))
            .ForMember(dest => dest.ImageUrl, opt =>
            opt.MapFrom<ImageUrlResolver<Course, CourseQueryDto>>())
            .ForMember(dest => dest.Category,
            opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null));

            CreateMap<CourseCommandDto, Course>()
            .ForMember(dest => dest.Category, opt => opt.Ignore());

            CreateMap<CourseCommandEditDto, Course>()
            .ForMember(dest => dest.Name,
                opt => opt.Condition((src, dest, srcMember) => srcMember != null))
            .ForMember(dest => dest.Price, opt =>
                opt.Condition(src => src.Price.HasValue))
            .ForMember(dest => dest.ImageFileName,
                opt => opt.Condition((src, dest, srcMember) => srcMember != null))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CategoryId, opt => opt.Ignore());
        }

    }
}
