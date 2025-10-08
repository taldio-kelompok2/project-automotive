using AutoMapper;
using AutomotiveApp.Application.Mapper.Resolver;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;

namespace AutomotiveApp.Application.Mapper
{
    public class CourseSessionProfile : Profile
    {
        public CourseSessionProfile()
        {
            CreateMap<CourseSession, CourseSessionQueryDto>()
            .ForMember(dest => dest.Course,
                opt => opt.MapFrom(src => src.Course.Name));

            CreateMap<CourseSessionQueryDto, CourseSession>()
            .ForMember(dest => dest.Course,
                opt => opt.Ignore());

            CreateMap<CourseSessionCommandDto, CourseSession>();

            CreateMap<CourseSessionEditCommandDto, CourseSession>();
        }

    }
}
