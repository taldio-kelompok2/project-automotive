using MediatR;

namespace AutomotiveApp.Application.Features.CourseSessions.Commands
{
    public record DeleteCourseSessionCommand(Guid Id) : IRequest;
}