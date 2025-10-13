using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Carts;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.Cart
{
    public class CartCreateDtoValidator : AbstractValidator<CartCreateDto>
    {
        private readonly IUnitOfWork _uow;
        public CartCreateDtoValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("User Id is required.")
                .MustAsync(async (dto, id, ct) => await _uow.UserRepo.DataExistAsync(u => u.Id == id, ct: ct))
                .WithMessage(dto => $"User Id {dto.UserId} didn't exist.")
                .DependentRules(() =>
                {
                    RuleFor(x => x.UserId)
                        .MustAsync(async (dto, id, ct) =>
                            !await _uow.CartRepo.DataExistAsync(c => c.UserId == id, ct: ct))
                        .WithMessage(dto => $"User Id {dto.UserId} already has a cart.");
                });
        }
    }
}