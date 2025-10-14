using AutomotiveApp.Application.Interfaces;
using AutomotiveApp.Shared.Dtos.Carts;
using FluentValidation;

namespace AutomotiveApp.WebAPI.Validators.Carts
{
    public class CartDetailsReadDtoValidator : AbstractValidator<CartReadDetailsDto>
    {
        private readonly IUnitOfWork _uow;
        public CartDetailsReadDtoValidator(IUnitOfWork uow)
        {
            _uow = uow;

            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Cart Id is required.")
                .MustAsync(async (dto, id, ct) => await _uow.CartRepo.DataExistAsync(c => c.Id == id, ct: ct))
                .WithMessage(dto => $"Cart Id {dto.Id} dosent exist");
        }
    }
}