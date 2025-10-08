using AutomotiveApp.Application.Features.CourseBookings.Commands;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Courses;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class UpdateCourseBookingCourseCommandValidator
        : AbstractValidator<EditCourseBookingCourseCommand>
    {
        private readonly IUnitOfWork _uow;

        public UpdateCourseBookingCourseCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.BookingId)
                .MustAsync(async (id, ct) =>
                    await _uow.CourseBookingRepo.DataExistAsync(b => b.Id == id, ct))
                .WithMessage(id => $"Course booking with Id {id} does not exist.");

            RuleFor(x => x.NewSessionId)
                .MustAsync(async (sessionId, ct) =>
                    await _uow.CourseSessionRepo.DataExistAsync(c => c.Id == sessionId, ct))
                .WithMessage(Booking => $"Course with Id {Booking.NewSessionId} does not exist.");
        }
    }
}