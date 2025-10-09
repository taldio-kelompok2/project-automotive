using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseSessions.Queries
{
    public record GetCourseSessions(Guid? CourseId) : IRequest<IEnumerable<CourseSessionQueryDto>>;
}