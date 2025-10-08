using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.WebAPI.Dto.Courses;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.Course
{
    public class CourseCreateRequestValidator : AbstractValidator<CourseCreateRequest>
    {
        private readonly IUnitOfWork _uow;
        private static readonly string[] ImageExtension = [".jpg", ".jpeg", ".png", ".svg"];

        public CourseCreateRequestValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MustAsync(async (name, ct) => { return !await _uow.CourseRepo.DataExistAsync(c => c.Name == name, ct); })
                .WithMessage("Name must be unique.");

            RuleFor(x => (int)x.Price)
                .NotEmpty().WithMessage("Price is required.")
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category is required.")
                .MustAsync(async (id, ct) => { return await _uow.CourseCategoryRepo.DataExistAsync(cc => cc.Id == id, ct); })
                .WithMessage(x => $"Category Id {x.CategoryId} dosent exist");

            RuleFor(x => x.Image)
                .Must(f => f == null || ImageExtension.Contains(Path.GetExtension(f.FileName).ToLower()))
                .WithMessage("Invalid image file type. Only JPG, JPEG, PNG, or SVG formats are supported.");
        }

    }
}