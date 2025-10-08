using AutomotiveApp.Application.Features.CourseBookings.Commands;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.WebAPI.Validators.CourseBookings
{
    public class AddCourseBookingCommandValidator
        : AbstractValidator<AddCourseBookingCommand>
    {
        private readonly IUnitOfWork _uow;
        private readonly UserManager<User> _userManager;

        public AddCourseBookingCommandValidator(IUnitOfWork uow, UserManager<User> userManager)
        {
            _uow = uow;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));

            RuleFor(x => x.Data.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .MustAsync(async (dto, userId, ct) =>
                {
                    var user = await _userManager.FindByIdAsync(userId.ToString());
                    return user != null;
                })
                .WithMessage(dto => $"User with Id {dto.Data.UserId} does not exist.");

            RuleFor(x => x.Data.SessionId)
                .NotEmpty().WithMessage("CourseSessionId is required.")
                .MustAsync(async (dto, sessionId, ct) =>
                    await _uow.CourseSessionRepo.DataExistAsync(s => s.Id == sessionId, ct))
                .WithMessage(dto => $"Course session with Id {dto.Data.SessionId} does not exist.");
        }
    }
}
