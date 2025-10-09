using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Queries
{
    public record GetCourseCategoriesPaged(int Page = 1, int ItemTaken = 6)
    : IRequest<PaginatedResult<CourseCategoryQueryDto>>;

}