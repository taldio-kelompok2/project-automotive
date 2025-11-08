using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.WebAPI.Dto.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.Courses.Queries
{
    public record GetCourseStatistic(int? Year)
    : IRequest<IEnumerable<CourseStatisticDto>>;

}