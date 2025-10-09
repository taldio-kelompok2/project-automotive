using AutomotiveApp.Application.Features.CourseBookings.Queries;
using AutomotiveApp.Application.Features.CourseSessions.Queries;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class GetCourseBookingsValidator
    : AbstractValidator<GetCourseBookings>
    {
        private readonly IUnitOfWork _uow;
        public GetCourseBookingsValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.SessionId)
            .MustAsync(async (sessionId, ct) =>
            {
                if (sessionId == null) return true;
                return await _uow.CourseSessionRepo.DataExistAsync(s => s.Id == sessionId, ct: ct);
            })
            .WithMessage("Course Session does not exist.");

        }

    }
}