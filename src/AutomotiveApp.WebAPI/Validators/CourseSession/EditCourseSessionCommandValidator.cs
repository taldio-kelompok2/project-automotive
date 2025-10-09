using AutomotiveApp.Application.Features.CourseSessions.Commands;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class EditCourseSessionCommandValidator
    : AbstractValidator<EditCourseSessionCommand>
    {
        private readonly IUnitOfWork _uow;
        public EditCourseSessionCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.NewData.Id)
            .MustAsync(async (dto, id, ct) =>
                await _uow.CourseSessionRepo.DataExistAsync(c => c.Id == id, ct: ct))
            .WithMessage(dto => $"Course Session Id {dto.NewData.Id} does not exist.");

            RuleFor(x => x.NewData.Date)
                .NotEmpty().WithMessage("Course Session Date is required.")
                .Must(date => date.Date >= DateTime.UtcNow.Date)
                    .WithMessage("Course Session Date must be today or a future date.")
                .MustAsync(async (dto, date, ct) =>
                    !await _uow.CourseSessionRepo.DataExistAsync(
                        c => c.CourseId == dto.NewData.CourseId && c.Date.Date == date.Date && c.Id != dto.NewData.Id, ct: ct))
                    .WithMessage(dto => $"A session for Course Id {dto.NewData.CourseId} already exists on {dto.NewData.Date:yyyy-MM-dd}.");

            RuleFor(x => (int)x.NewData.Capacity)
                .NotEmpty().WithMessage("Course Session Capacity is required")
                .When(x => x.NewData.Capacity != 0)
                .GreaterThan(0).WithMessage("Course Session Capacity must be greater than 0.");

            RuleFor(x => x.NewData.CourseId)
                .NotEmpty().WithMessage("Course Id is required.")
                .MustAsync(async (dto, courseId, ct) => await _uow.CourseRepo.DataExistAsync(cc => cc.Id == courseId, ct: ct))
                .When(x => x.NewData.CourseId != Guid.Empty)
                .WithMessage(dto => $"Course Id {dto.NewData.CourseId} dosent exist");
        }
    }
}