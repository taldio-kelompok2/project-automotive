using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Commands
{
    public record DeleteCourseCommand(Guid Id) : IRequest;
}