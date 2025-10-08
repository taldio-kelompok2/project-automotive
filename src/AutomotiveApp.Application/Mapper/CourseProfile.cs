using AutoMapper;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;

namespace AutomotiveApp.Application.Mapper
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseQueryDto>()
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category.Name.ToString()));

            CreateMap<CourseCommandDto, Course>()
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        }

    }
}
