using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public record GetCoursesPaged(int Page = 1, int ItemTaken = 6, bool IsRandom = false)
    : IRequest<PaginatedResult<CourseQueryDto>>;

}