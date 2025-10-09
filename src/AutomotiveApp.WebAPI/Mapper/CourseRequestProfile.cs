using AutoMapper;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.WebAPI.Dto.Courses;

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
            CreateMap<CourseEditRequest, CourseCommandEditDto>()
            .ForAllMembers(opt =>
            {
                opt.Condition((src, dest, srcMember) => srcMember != null);
            });
        }
    }
}