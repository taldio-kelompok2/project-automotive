using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.User
{
    public class UserUpdateDto
    {
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4-50 characters")]
        public string? UserName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Must be an Email")]
        public string? Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        public bool? Status { get; set; } = true;
    }
}
