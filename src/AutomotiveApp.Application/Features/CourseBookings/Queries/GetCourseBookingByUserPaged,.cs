using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseBookings.Queries
{
    public record GetCourseBookingByUserPaged(Guid UserId, Guid? CourseId, int Page = 1, int ItemTaken = 6) : IRequest<PaginatedResult<CourseBookingQueryDto>>;

}