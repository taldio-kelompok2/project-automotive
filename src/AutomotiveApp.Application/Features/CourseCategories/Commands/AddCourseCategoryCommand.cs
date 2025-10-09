using AutomotiveApp.Shared.Dtos.Courses;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Commands
{
    public record AddCourseCategoryCommand(CourseCategoryCommandDto Data) : IRequest<CourseCategoryQueryDto>;
}