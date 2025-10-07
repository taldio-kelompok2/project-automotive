using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public record GetCourseById(Guid Id) : IRequest<CourseQueryDto>;

}