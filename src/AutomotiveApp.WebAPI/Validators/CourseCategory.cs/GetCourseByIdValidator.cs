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
                .MustAsync(async (dto, id, ct) => await IdExists(id))
                .WithMessage(dto => $"Course Category Id {dto.Id} dosent exist");

        }

        private async Task<bool> IdExists(Guid id)
        {
            return await _uow.CourseCategoryRepo.DataExistAsync(id);
        }

    }
}