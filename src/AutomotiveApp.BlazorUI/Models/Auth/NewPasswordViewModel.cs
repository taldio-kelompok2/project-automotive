using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.BlazorUI.Models.Auth
{
    public class NewPasswordViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters")]
        public string Password { get; set; } = null!;

        [Required(ErrorMessage = "Confirm Password is required")]
        public string ConfirmPassword { get; set; } = null!;
        public bool Success { get; set; } = false;
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