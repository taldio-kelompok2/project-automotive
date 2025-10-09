using AutomotiveApp.Application.Features.CourseCategories.Queries;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseCategory
{
    public class GetCategoryCourseByIdValidator
    : AbstractValidator<GetCourseCategoryById>
    {

        private readonly IUnitOfWork _uow;
        public GetCategoryCourseByIdValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Course Category Id is required.")
                .MustAsync(async (dto, id, ct) => await _uow.CourseCategoryRepo.DataExistAsync(cc => cc.Id == id, ct: ct))
                .WithMessage(dto => $"Course Category Id {dto.Id} dosent exist");

        }
    }
}