using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseBookings.Commands
{
    public record EditCourseBookingCourseCommand(Guid BookingId, Guid NewSessionId) : IRequest<CourseBookingQueryDto>;

}