using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Queries
{
    public record GetCourseCategoryById(Guid Id) : IRequest<CourseCategoryQueryDto>;
}