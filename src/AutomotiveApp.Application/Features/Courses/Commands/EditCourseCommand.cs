using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Commands
{
    public record EditCourseCommand(CourseCommandEditDto NewData) : IRequest<CourseQueryDto>;
}