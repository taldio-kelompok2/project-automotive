using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.WebAPI.Dto.Courses;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CourseCategory
{
    public class CourseCategoryEditRequestValidator
    : AbstractValidator<CourseCategoryEditRequest>
    {

        private readonly IUnitOfWork _uow;
        private static readonly string[] ImageExtension = [".jpg", ".jpeg", ".png", ".svg"];
        public CourseCategoryEditRequestValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Name)
                .MustAsync(async (cce, name, ct) => { return !await _uow.CourseCategoryRepo.DataExistAsync(c => c.Name == name && c.Id != cce.Id, ct); })
                .WithMessage("Name must be unique.");

            RuleFor(x => x.Image)
                .Must(f => f == null || ImageExtension.Contains(Path.GetExtension(f.FileName).ToLower()))
                .WithMessage("Invalid image file type. Only JPG, JPEG, PNG, or SVG formats are supported.");

        }
    }
}