using AutomotiveApp.Application.Features.CourseCategories.Queries;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.WebAPI.Dto.Courses;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseCategory
{
    public class CourseCategoryCreateRequestValidator
    : AbstractValidator<CourseCategoryCreateRequest>
    {

        private readonly IUnitOfWork _uow;
        private static readonly string[] ImageExtension = [".jpg", ".jpeg", ".png", ".svg"];
        public CourseCategoryCreateRequestValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MustAsync(async (name, ct) =>
                {
                    return !await _uow.CourseCategoryRepo.DataExistAsync(
                    c => c.Name == name, ct: ct);
                })
                .WithMessage("Name must be unique.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");

            RuleFor(x => x.Image)
                .Must(f => f == null || ImageExtension.Contains(Path.GetExtension(f.FileName).ToLower()))
                .WithMessage("Invalid image file type. Only JPG, JPEG, PNG, or SVG formats are supported.");

        }
    }
}