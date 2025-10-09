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
            .MustAsync(async (courseId, ct) =>
            {
                if (courseId == null) return true;
                return await _uow.CourseRepo.DataExistAsync(c => c.Id == courseId, ct: ct);
            })
            .WithMessage("Course does not exist.");

        }

    }
}