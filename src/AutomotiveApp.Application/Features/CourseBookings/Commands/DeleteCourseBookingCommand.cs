using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseBookings.Commands
{
    public record DeleteCourseBookingCommand(Guid Id) : IRequest;
}