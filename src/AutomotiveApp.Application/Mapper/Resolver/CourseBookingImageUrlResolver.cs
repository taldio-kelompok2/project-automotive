using AutoMapper;
using AutomotiveApp.Application.Helpers;
using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;

namespace AutomotiveApp.Application.Mapper.Resolver
{
    public class CourseBookingImageUrlResolver(UrlGeneratorHelper urlHelper)
        : IValueResolver<CourseBooking, CourseBookingQueryDto, string?>
    {
        public string? Resolve(CourseBooking source, CourseBookingQueryDto destination, string? destMember, ResolutionContext context)
        {
            var course = source.Session?.Course;
            return course == null ? null : urlHelper.GeneratePublicUrl<Course>(course.ImageFileName);
        }
    }
}
