using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.WebAPI.Validators.User
{
    public class UserCreateRequestDtoValidator : AbstractValidator<UserCreateRequestDto>
    {
        private readonly UserManager<AutomotiveApp.Domain.Entities.Auth.User> _userManager;
        public UserCreateRequestDtoValidator(UserManager<AutomotiveApp.Domain.Entities.Auth.User> userManager)
        {
            _userManager = userManager;

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Name is required")
                .Length(4, 50).WithMessage("Username must be between 4–50 characters")
                .MustAsync(BeUniqueUserName).WithMessage("Username already exists");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(BeUniqueEmail).WithMessage("Email is already registered");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters");

            RuleFor(u => u.PhoneNumber)
                .Matches(@"^\+?[0-9\s\-]+$")
                .When(u => !string.IsNullOrWhiteSpace(u.PhoneNumber))
                .WithMessage("Invalid phone number format");

            RuleFor(u => u.Role)
                .NotEmpty().WithMessage("Role is required")
                .Must(r => r == "Admin" || r == "Buyer")
                .WithMessage("Role must be either 'Admin' or 'Buyer'");

            RuleFor(u => u.Status)
                .NotNull().WithMessage("Status is required");
        }

        private async Task<bool> BeUniqueUserName(string userName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return true; // handled by NotEmpty rule already

            var existingUser = await _userManager.FindByNameAsync(userName);
            return existingUser == null;
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true; // handled by NotEmpty rule already

            var existingUser = await _userManager.FindByEmailAsync(email);
            return existingUser == null;
        }
    }
}
