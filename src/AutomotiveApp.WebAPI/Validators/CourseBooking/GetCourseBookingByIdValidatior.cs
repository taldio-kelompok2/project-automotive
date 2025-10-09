using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Courses;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseSession
{
    public class GetCourseBookingByIdValidator
    : AbstractValidator<CourseBookingQueryDto>
    {
        private readonly IUnitOfWork _uow;
        public GetCourseBookingByIdValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Course Booking Id is required.")
                .MustAsync(async (dto, id, ct) => await _uow.CourseBookingRepo.DataExistAsync(cc => cc.Id == id, ct: ct))
                .WithMessage(dto => $"Course Booking Id {dto.Id} dosent exist");

        }
    }
}