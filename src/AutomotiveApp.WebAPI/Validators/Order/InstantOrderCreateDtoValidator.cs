using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.CartItems;
using AutomotiveApp.Shared.Dtos.Order;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.WebAPI.Validators.Order
{
    public class InstantOrderCreateDtoValidator : AbstractValidator<InstantOrderCreateDto>
    {
        private readonly IUnitOfWork _uow;
        public InstantOrderCreateDtoValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.PaymentId)
                .NotEmpty().WithMessage("Payment method is required.")
                .MustAsync(async (paymentId, ct) =>
                    await _uow.PaymentRepo.DataExistAsync(p => p.Id == paymentId, ct: ct))
                .WithMessage("Payment method not found.");

            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("Session is required.")
                .MustAsync(async (sessionId, ct) =>
                    await _uow.CourseSessionRepo.Query()
                        .AnyAsync(s => s.Id == sessionId && s.Date > DateTime.UtcNow, ct))
                .WithMessage("Session must exist and be scheduled for a future date.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User is required.")
                .MustAsync(async (userId, ct) =>
                    await _uow.UserRepo.DataExistAsync(u => u.Id == userId, ct: ct))
                .WithMessage("User not found.");

            RuleFor(x => x)
                .MustAsync(NoDuplicateSession)
                .WithMessage("You already have an order for this session.");

            RuleFor(x => x)
                .MustAsync(NoConflictingBookings)
                .WithMessage("You already have a booking on the same date.");
        }

        private async Task<bool> NoDuplicateSession(InstantOrderCreateDto dto, CancellationToken ct)
        {
            return !await _uow.OrderItemRepo.Query()
                .Include(oi => oi.Order)
                .AnyAsync(oi =>
                    oi.SessionId == dto.SessionId &&
                    oi.Order.UserId == dto.UserId, ct);
        }

        private async Task<bool> NoConflictingBookings(InstantOrderCreateDto dto, CancellationToken ct)
        {
            var session = await _uow.CourseSessionRepo.Query()
                .Where(s => s.Id == dto.SessionId)
                .Select(s => s.Date)
                .FirstOrDefaultAsync(ct);

            if (session == default) return true;

            return !await _uow.CourseBookingRepo.Query()
                .Include(b => b.Session)
                .AnyAsync(b =>
                    b.UserId == dto.UserId &&
                    b.Session.Date.Date == session.Date.Date, ct);
        }
    }
}
