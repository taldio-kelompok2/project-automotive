using AutomotiveApp.Application.Features.CourseSessions.Queries;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class GetSessionsByIdValidator
    : AbstractValidator<GetCourseSessions>
    {
        private readonly IUnitOfWork _uow;
        public GetSessionsByIdValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.CourseId)
                .MustAsync(CourseExists)
                .WithMessage(x => $"CourseId {x.CourseId} does not exist")
                .When(x => x.CourseId.HasValue);
        }

        private async Task<bool> CourseExists(Guid? courseId, CancellationToken ct)
        {
            if (!courseId.HasValue) return true;
            return await _uow.CourseRepo.DataExistAsync(c => c.Id == courseId.Value, ct);
        }
    }
}