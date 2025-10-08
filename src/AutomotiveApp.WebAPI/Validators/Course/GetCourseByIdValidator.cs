using AutomotiveApp.Application.Features.Courses.Queries;
using AutomotiveApp.Application.Interfaces;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.Course
{
    public class GetCourseByIdValidator : AbstractValidator<GetCourseById>
    {
        private readonly IUnitOfWork _uow;
        private readonly ILogger<GetCourseByIdValidator> _logger;
        public GetCourseByIdValidator(IUnitOfWork uow, ILogger<GetCourseByIdValidator> logger)
        {
            _uow = uow;
            _logger = logger;

            _logger.LogInformation("Validators has been instantiated");

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Course Id is required.")
                .MustAsync(async (dto, id, ct) => await IdExists(id))
                .WithMessage(dto => $"Course Id {dto.Id} dosent exist");

        }

        private async Task<bool> IdExists(Guid id)
        {
            return await _uow.CourseRepo.DataExistAsync(c => c.Id == id);
        }

    }
}