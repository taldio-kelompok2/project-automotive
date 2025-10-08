using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseBookings.Commands
{
    public record AddCourseBookingCommand(CourseBookingCommandDto Data) : IRequest<CourseBookingQueryDto>;
}