using AutomotiveApp.Application.Features.CourseSessions.Commands;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class AddCourseSessionCommandValidator
    : AbstractValidator<AddCourseSessionCommand>
    {
        private readonly IUnitOfWork _uow;
        public AddCourseSessionCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Data.Date)
                .NotEmpty().WithMessage("Course Session Date is required.")
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                    .WithMessage("Course Session Date must be today or a future date.")
                .MustAsync(async (dto, date, ct) =>
                    !await _uow.CourseSessionRepo.DataExistAsync(
                        c => c.CourseId == dto.Data.CourseId && c.Date.Date == date.Date, ct: ct))
                    .WithMessage(dto => $"A session for Course Id {dto.Data.CourseId} already exists on {dto.Data.Date:yyyy-MM-dd}.");

            RuleFor(x => (int)x.Data.Capacity)
                .NotEmpty().WithMessage("Course Session Capacity is required")
                .When(x => x.Data.Capacity != 0)
                .GreaterThan(0).WithMessage("Course Session Capacity must be greater than 0.");

            RuleFor(x => x.Data.CourseId)
                .NotEmpty().WithMessage("Course Id is required.")
                .MustAsync(async (dto, courseId, ct) => await _uow.CourseRepo.DataExistAsync(cc => cc.Id == courseId, ct: ct))
                .When(x => x.Data.CourseId != Guid.Empty)
                .WithMessage(dto => $"Course Id {dto.Data.CourseId} dosent exist");

        }
    }
}