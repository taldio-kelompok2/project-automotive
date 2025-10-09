using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Commands
{
    public record DeleteCourseCategoryCommand(Guid Id) : IRequest;
}