using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.User
{
    public class UserCreateDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4-50 characters")]
        public string UserName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Must be an Email")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }

        public bool Status { get; set; } = true;
    }
}
