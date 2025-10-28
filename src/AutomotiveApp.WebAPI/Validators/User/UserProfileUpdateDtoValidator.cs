using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.WebAPI.Validators.User
{
    public class UserProfileUpdateDtoValidator : AbstractValidator<UserProfileUpdateDto>
    {
        private readonly UserManager<AutomotiveApp.Domain.Entities.Auth.User> _userManager;
        public UserProfileUpdateDtoValidator(UserManager<AutomotiveApp.Domain.Entities.Auth.User> userManager)
        {
            _userManager = userManager;

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required.")
                .Length(4, 50).WithMessage("Username must be between 4 and 50 characters.")
                .MustAsync(async (dto, username, ct) =>
                {
                    var exists = await userManager.Users
                        .AnyAsync(u => u.UserName == username && u.Id != dto.CurrentUserId, ct);
                    return !exists;
                })
                .When(u => !string.IsNullOrWhiteSpace(u.UserName))
                .WithMessage("Username is already in use.");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email format.")
                .MustAsync(async (dto, email, ct) =>
                {
                    var exists = await userManager.Users
                        .AnyAsync(u => u.Email == email && u.Id != dto.CurrentUserId, ct);
                    return !exists;
                })
                .When(u => !string.IsNullOrWhiteSpace(u.Email))
                .WithMessage("Email is already in use.");

            RuleFor(u => u.PhoneNumber)
                .Matches(@"^\+?[0-9\s\-]+$")
                .When(u => !string.IsNullOrWhiteSpace(u.PhoneNumber))
                .WithMessage("Invalid phone number format.");
        }
    }
}


