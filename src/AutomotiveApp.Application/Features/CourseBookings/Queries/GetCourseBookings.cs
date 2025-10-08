using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseBookings.Queries
{
    public record GetCourseBookings() : IRequest<IEnumerable<CourseBookingQueryDto>>;

}