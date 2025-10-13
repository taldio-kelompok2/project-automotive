using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseBookings.Queries
{
    public record GetCourseBookingByUser(Guid UserId) : IRequest<IEnumerable<CourseBookingQueryDto>>;

}