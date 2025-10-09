using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseSessions.Commands
{
    public record EditCourseSessionCommand(CourseSessionEditCommandDto NewData) : IRequest<CourseSessionQueryDto>;
}