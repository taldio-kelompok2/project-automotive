using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.CartItems;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.CartItem
{
    public class CartItemUpdateDtoValidator : AbstractValidator<CartItemUpdateDto>
    {
        private readonly IUnitOfWork _uow;
        public CartItemUpdateDtoValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Id)
            .MustAsync(async (id, ct) => await _uow.CartItemRepo.DataExistAsync(c => c.Id == id, ct: ct))
            .WithMessage(x => $"CartItem with Id {x.Id} does not exist.");

            RuleFor(x => x.SessionId)
                .MustAsync(async (dto, sessionId, ct) =>
                    await _uow.CourseSessionRepo.DataExistAsync(s => s.Id == sessionId, ct: ct))
                .WithMessage(dto => $"Session Id {dto.SessionId} doesn't exist.");

        }
    }
}
