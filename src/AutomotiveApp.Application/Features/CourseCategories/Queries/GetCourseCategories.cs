using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Queries
{
    public record GetCourseCategories() : IRequest<IEnumerable<CourseCategoryQueryDto>>;

}