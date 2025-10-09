using AutomotiveApp.Application.Features.Courses.Commands;
using AutomotiveApp.Application.Features.CourseSessions.Commands;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;

public class DeleteCourseSessionCommandHandler(IUnitOfWork _uow)
: IRequestHandler<DeleteCourseSessionCommand>
{
    public async Task<Unit> Handle(DeleteCourseSessionCommand request, CancellationToken ct)
    {
        var data = await _uow.CourseSessionRepo.GetByIdAsync(request.Id, ct: ct)
        ?? throw new NotFoundException<CourseSession>(request.Id);

        _uow.CourseSessionRepo.Delete(data);
        var saved = await _uow.SaveChangesAsync(ct);
        if (saved > 0) return Unit.Value;

        throw new InvalidOperationException("Unable to delete the course Session, Please try again.");

    }
}