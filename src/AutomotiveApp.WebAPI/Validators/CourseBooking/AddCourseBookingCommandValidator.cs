using AutomotiveApp.Application.Features.CourseBookings.Commands;
using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Application.Interfaces.Repositories;
using AutomotiveApp.Domain.Entities.Auth;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.WebAPI.Validators.CourseBookings
{
    public class AddCourseBookingCommandValidator
        : AbstractValidator<AddCourseBookingCommand>
    {
        private readonly IUnitOfWork _uow;

        public AddCourseBookingCommandValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Data.UserId)
                .NotEmpty().WithMessage("UserId is required.")
                .MustAsync(async (dto, userId, ct) =>
                {
                    var user = await _uow.UserRepo.GetByIdAsync(userId, ct: ct);
                    return user != null;
                })
                .WithMessage(dto => $"User with Id {dto.Data.UserId} does not exist.");

            RuleFor(x => x.Data.SessionId)
                .NotEmpty().WithMessage("CourseSessionId is required.")
                .MustAsync(async (dto, sessionId, ct) =>
                    await _uow.CourseSessionRepo.DataExistAsync(s => s.Id == sessionId, ct: ct))
                .WithMessage(dto => $"Course session with Id {dto.Data.SessionId} does not exist.")
                .DependentRules(() =>
                {
                    // Check session availability
                    RuleFor(x => x.Data.SessionId)
                        .MustAsync(async (dto, sessionId, ct) =>
                            await _uow.CourseSessionRepo.IsSessionAvailable(sessionId, ct))
                        .WithMessage(dto => $"Course session with Id {dto.Data.SessionId} is full.");

                    // Check if user already booked this session
                    RuleFor(x => x.Data.SessionId)
                        .MustAsync(async (dto, sessionId, ct) =>
                            !await _uow.CourseBookingRepo
                                .DataExistAsync(b => b.UserId == dto.Data.UserId && b.SessionId == sessionId, ct: ct))
                        .WithMessage(dto => $"User with Id {dto.Data.UserId} has already booked this session.");
                });
        }
    }
}
