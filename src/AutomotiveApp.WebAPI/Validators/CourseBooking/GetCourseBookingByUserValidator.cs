using AutomotiveApp.Application.Features.CourseBookings.Queries;
using AutomotiveApp.Application.Features.CourseSessions.Queries;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class GetCourseBookingByUserValidator
    : AbstractValidator<GetCourseBookingByUser>
    {
        private readonly IUnitOfWork _uow;
        public GetCourseBookingByUserValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.CourseId)
            .MustAsync(async (courseId, ct) =>
            {
                if (courseId == null) return true;
                return await _uow.CourseRepo.DataExistAsync(s => s.Id == courseId, ct: ct);
            })
            .WithMessage("Course does not exist.");

        }

    }
}