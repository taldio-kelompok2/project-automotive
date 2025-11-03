using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Exceptions;
using MediatR;

namespace AutomotiveApp.Application.Features.CourseSessions.Commands
{
    public class DeleteCourseSessionCommandHandler(IUnitOfWork _uow)
    : IRequestHandler<DeleteCourseSessionCommand>
    {
        public async Task<Unit> Handle(DeleteCourseSessionCommand request, CancellationToken cancellationToken)
        {
            var data = await _uow.CourseSessionRepo.GetByIdAsync(request.Id, ct: cancellationToken)
            ?? throw new NotFoundException<CourseSession>(request.Id);

            _uow.CourseSessionRepo.Delete(data);
            var saved = await _uow.SaveChangesAsync(cancellationToken);
            if (saved > 0) return Unit.Value;

            throw new InvalidOperationException("Unable to delete the course Session, Please try again.");

        }
    }
}