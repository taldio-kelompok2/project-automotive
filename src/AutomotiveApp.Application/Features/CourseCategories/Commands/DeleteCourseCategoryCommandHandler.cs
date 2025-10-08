using AutoMapper;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseCategories.Commands
{
    public class DeleteCourseCategoryCommandHandler(IUnitOfWork _uow)
    : IRequestHandler<DeleteCourseCategoryCommand>
    {
        public async Task<Unit> Handle(DeleteCourseCategoryCommand request, CancellationToken ct)
        {
            var data = await _uow.CourseCategoryRepo.GetByIdAsync(request.Id, ct: ct)
            ?? throw new NotFoundException<CourseCategory>($"Course with ID {request.Id} was not found.");

            _uow.CourseCategoryRepo.Delete(data);
            var saved = await _uow.SaveChangesAsync(ct);
            if (saved > 0) return Unit.Value;

            throw new InvalidOperationException("Unable to delete the course category, Please try again.");
        }
    }
}