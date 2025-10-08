using AutomotiveApp.Application.Features.CourseSessions.Queries;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class GetCourseSessionByIdValidator
    : AbstractValidator<GetCourseSessionById>
    {
        private readonly IUnitOfWork _uow;
        public GetCourseSessionByIdValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Course Session Id is required.")
                .MustAsync(async (dto, id, ct) => await _uow.CourseSessionRepo.DataExistAsync(cc => cc.Id == id, ct: ct))
                .WithMessage(dto => $"Course Session Id {dto.Id} dosent exist");

        }
    }
}