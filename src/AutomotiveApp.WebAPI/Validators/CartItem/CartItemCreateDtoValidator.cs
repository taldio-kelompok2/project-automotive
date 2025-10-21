using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.CartItems;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CartItem
{
    public class CartItemCreateDtoValidator : AbstractValidator<CartItemCreateDto>
    {
        private readonly IUnitOfWork _uow;
        public CartItemCreateDtoValidator(IUnitOfWork uow)
        {
            _uow = uow;

            // cart required, exist
            RuleFor(x => x.CartId)
                .NotEmpty().WithMessage("Cart Id is required.")
                .MustAsync(async (dto, cartId, ct) =>
                    await _uow.CartRepo.DataExistAsync(ci => ci.Id == cartId, ct: ct)
                ).WithMessage(dto => $"Cart Id {dto.CartId} doesn't exist.");

            // Session required, exist
            RuleFor(x => x.SessionId)
                .NotEmpty().WithMessage("Session Id is required.")
                .MustAsync(async (dto, sessionId, ct) =>
                    await _uow.CourseSessionRepo.DataExistAsync(s => s.Id == sessionId, ct: ct))
                .WithMessage(dto => $"Session Id {dto.SessionId} doesn't exist.");

            RuleFor(x => x.SessionId)
                .MustAsync(async (dto, sessionId, ct) =>
                    !await _uow.CartItemRepo.DataExistAsync(s => s.SessionId == sessionId && s.CartId == dto.CartId, ct: ct))
                .WithMessage(dto => $"Session Id {dto.SessionId} already exist in cart.")
                .WhenAsync(async (dto, ct) =>
                    await _uow.CourseSessionRepo.DataExistAsync(s => s.Id == dto.SessionId, ct: ct));

        }
    }
}
