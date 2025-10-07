using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public record GetCourses() : IRequest<IEnumerable<CourseQueryDto>>;

}