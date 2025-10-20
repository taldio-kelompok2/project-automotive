using AutoMapper;
using AutomotiveApp.Application.Mapper.Resolver;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;

namespace AutomotiveApp.Application.Mapper
{
    public class CourseBookingProfile : Profile
    {
        public CourseBookingProfile()
        {
            CreateMap<CourseBooking, CourseBookingQueryDto>()
            .ForMember(dest => dest.Session, opt => opt.MapFrom(src => src.Session.Date))
            .ForMember(dest => dest.Course, opt => opt.MapFrom(src => src.Session.Course.Name))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Session.Course.Category != null ? src.Session.Course.Category.Name : null))
            .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<CourseBookingImageUrlResolver>())
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId.ToString()))
            .ForMember(dest => dest.SessionId, opt => opt.MapFrom(src => src.SessionId.ToString()));

            CreateMap<CourseBookingCommandDto, CourseBooking>();
        }

    }
}
