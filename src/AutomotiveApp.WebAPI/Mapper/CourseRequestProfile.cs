using AutoMapper;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.WebAPI.Dto;

namespace AutomotiveApp.WebAPI.Mapper
{
    public class CourseRequestProfile : Profile
    {
        public CourseRequestProfile()
        {
            CreateMap<CourseCreateRequest, CourseCommandDto>()
            .ForMember(
                dest => dest.ImageFilename,
                opt => opt.MapFrom(src => src.Image != null ? src.Image.FileName : null)
            );
        }
    }
}