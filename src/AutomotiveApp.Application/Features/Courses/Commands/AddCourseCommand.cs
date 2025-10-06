using AutomotiveApp.Shared.Dtos.Course;
using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Commands
{
    public record AddCourseCommand(CourseCommandDto Data) : IRequest<CourseQueryDto>;
}