using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.Auth
{
    public class RegisterRequestDto : IValidatableObject
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; } = null!;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != ConfirmPassword)
                yield return new ValidationResult(
                    "Passwords do not match",
                    [nameof(ConfirmPassword)]
                );
        }

    }


}