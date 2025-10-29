using AutomotiveApp.Application.Features.CourseBookings.Queries;
using AutomotiveApp.Application.Features.CourseSessions.Queries;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class GetCourseBookingByUserPagedValidator
    : AbstractValidator<GetCourseBookingByUserPaged>
    {
        private readonly IUnitOfWork _uow;
        public GetCourseBookingByUserPagedValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.CourseId)
            .MustAsync(async (courseId, ct) =>
            {
                if (courseId == null) return true;
                return await _uow.CourseRepo.DataExistAsync(s => s.Id == courseId, ct: ct);
            })
            .WithMessage("Course does not exist.");

            // RuleFor(x => x.Page)
            // .GreaterThan(0)
            // .When(x => x.Page.HasValue)
            // .WithMessage("Page must be a positive number.");

            // RuleFor(x => x.ItemTaken)
            //     .GreaterThan(0)
            //     .When(x => x.ItemTaken.HasValue)
            //     .WithMessage("Item Taken must be a positive number.");

        }

    }
}