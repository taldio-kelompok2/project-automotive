using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Shared.Dtos.Order;
using AutomotiveApp.Shared.Exceptions;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.WebAPI.Validators.Order
{
    public class OrderCreateDtoValidator : AbstractValidator<OrderCreateDto>
    {
        private readonly IUnitOfWork _uow;
        public OrderCreateDtoValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.PaymentMethodId)
                .NotEmpty().WithMessage("PaymentmethodId is required.")
                .MustAsync(async (paymentId, ct) =>
                    await _uow.PaymentRepo.DataExistAsync(p => p.Id == paymentId, ct: ct))
                .WithMessage("Payment method dosent exist.");

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .MustAsync(async (userId, ct) =>
                    await _uow.UserRepo.DataExistAsync(u => u.Id == userId, ct: ct))
                .WithMessage("User dosent exist.");

            RuleFor(x => x.CartItemIds)
            .NotEmpty().WithMessage("Cart items are required.")
            .MustAsync(async (cartItemIds, ct) =>
                await _uow.CartItemRepo.DataExistAsync(ci => cartItemIds.Contains(ci.Id), ct: ct))
            .WithMessage("One or more cart items do not exist.")
            .DependentRules(() =>
            {
                RuleFor(x => x)
                    .MustAsync(NoDuplicateSession)
                    .WithMessage("You already have an order for this session.");

                // RuleFor(x => x)
                //     .MustAsync(NoConflictingBookings)
                //     .WithMessage("You already have a booking on the same date.");
            });

            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage("CartId is required.")
                .MustAsync(async (cartId, ct) =>
                    await _uow.CartRepo.DataExistAsync(c => c.Id == cartId, ct: ct))
                .WithMessage("Cart dosent exist.");
        }

        private async Task<bool> NoDuplicateSession(OrderCreateDto dto, CancellationToken ct)
        {
            var cart = await _uow.CartRepo.FirstOrDefaultAsync
            (
                modifier: q => q.Include(c => c.Items),
                predicate: c => c.Id == dto.CartId,
                ct: ct
            ) ?? throw new NotFoundException<Cart>(dto.CartId);

            var selectedItems = cart.Items.Where(i => dto.CartItemIds.Contains(i.Id)).ToList();

            var sessionIds = selectedItems
            .Where(i => i.SessionId != Guid.Empty)
            .Select(i => i.SessionId)
            .ToList();

            return sessionIds.Count == sessionIds.Distinct().Count();
        }

        private async Task<bool> NoConflictingBookings(OrderCreateDto dto, CancellationToken ct)
        {
            var cart = await _uow.CartRepo.FirstOrDefaultAsync
            (
                modifier: q => q
                .Include(c => c.Items)
                .ThenInclude(i => i.Session),
                predicate: c => c.Id == dto.CartId,
                ct: ct
            ) ?? throw new NotFoundException<Cart>(dto.CartId);

            var userId = cart.UserId;

            var sessionDates = cart.Items
            .Where(i => i.Session.Date > DateTime.UtcNow)
            .Select(i => i.Session.Date.Date)
            .Distinct()
            .ToList();

            var hasConflict = await _uow.CourseBookingRepo.DataExistAsync(
                predicate: b =>
                    b.UserId == userId &&
                    sessionDates.Contains(b.Session.Date.Date),
                ct: ct
            );

            return !hasConflict;
        }
    }
}
