using AutomotiveApp.Application.Features.Courses.Commands;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;

public class DeleteCourseCommandHandler(IUnitOfWork _uow)
: IRequestHandler<DeleteCourseCommand>
{
    public async Task<Unit> Handle(DeleteCourseCommand request, CancellationToken ct)
    {
        var data = await _uow.CourseRepo.GetByIdAsync(request.Id, ct: ct)
        ?? throw new NotFoundException<Course>($"Course with ID {request.Id} was not found.");

        _uow.CourseRepo.Delete(data);
        var saved = await _uow.SaveChangesAsync(ct);
        if (saved > 0) return Unit.Value;

        throw new InvalidOperationException("Unable to delete the course, Please try again.");

    }
}