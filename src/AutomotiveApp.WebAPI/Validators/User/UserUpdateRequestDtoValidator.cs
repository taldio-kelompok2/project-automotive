using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Dtos.User;
using FluentValidation;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.WebAPI.Validators.User
{
    public class UserUpdateRequestDtoValidator : AbstractValidator<UserUpdateRequestDto>
    {
        private readonly UserManager<AutomotiveApp.Domain.Entities.Auth.User> _userManager;

        public UserUpdateRequestDtoValidator(UserManager<AutomotiveApp.Domain.Entities.Auth.User> userManager)
        {
            _userManager = userManager;

            RuleFor(u => u.UserName)
                .NotEmpty().WithMessage("Username is required")
                .Length(4, 50).WithMessage("Username must be between 4–50 characters")
                .MustAsync(BeUniqueUserName).WithMessage("Username already exists");

            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(BeUniqueEmail).WithMessage("Email is already registered");

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

        private async Task<bool> BeUniqueUserName(UserUpdateRequestDto dto, string userName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(userName))
                return true;

            var existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser == null)
                return true;

            // ✅ Allow same user to keep the same username
            if (existingUser.Id == dto.Id)
                return true;

            // ✅ Optional: Skip check if username hasn’t changed
            var currentUser = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (currentUser != null && currentUser.UserName == userName)
                return true;

            return false;
        }

        private async Task<bool> BeUniqueEmail(UserUpdateRequestDto dto, string email, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(email))
                return true;

            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser == null)
                return true;

            if (existingUser.Id == dto.Id)
                return true;

            // ✅ Optional: Skip check if email hasn’t changed
            var currentUser = await _userManager.FindByIdAsync(dto.Id.ToString());
            if (currentUser != null && currentUser.Email == email)
                return true;

            return false;
        }
    }
}
