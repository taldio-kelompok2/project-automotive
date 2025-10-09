using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Commands
{
    public record EditCourseCategoryCommand(CourseCategoryEditDto NewData) : IRequest<CourseCategoryQueryDto>;
}