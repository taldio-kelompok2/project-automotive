using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.WebAPI.Dto.Courses;
using FluentValidation;
using Microsoft.IdentityModel.Tokens;

namespace AutomotiveApp.WebAPI.Validators.Course
{
    public class CourseEditRequestValidator : AbstractValidator<CourseEditRequest>
    {
        private readonly IUnitOfWork _uow;
        private static readonly string[] ImageExtension = [".jpg", ".jpeg", ".png", ".svg"];
        public CourseEditRequestValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Name)
                .MustAsync(async (ecc, name, ct) =>
                {
                    // if (ecc.Name.IsNullOrEmpty()) return true;
                    if (string.IsNullOrEmpty(name)) return true;
                    return !await _uow.CourseRepo.DataExistAsync(
                        c => c.Name == name && c.Id != ecc.Id, ct: ct
                    );
                })
                .WithMessage("Name must be unique.");

            RuleFor(x => (int?)x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0");

            RuleFor(x => x.CategoryId)
                .MustAsync(async (id, ct) =>
                {
                    if (!id.HasValue) return true;
                    return await _uow.CourseCategoryRepo.DataExistAsync(cc => cc.Id == id, ct: ct);
                })
                .WithMessage(x => $"Category Id {x.CategoryId} dosent exist");

            RuleFor(x => x.Image)
                .Must(f => f == null || ImageExtension.Contains(Path.GetExtension(f.FileName).ToLower()))
                .WithMessage("Only jpg, jpeg, png, and svg images are allowed");
        }

    }
}