using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseSessions.Queries
{
    public record GetCourseSessionById(Guid Id) : IRequest<CourseSessionQueryDto>;
}