using System.ComponentModel.DataAnnotations;
using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name must not exceed 100 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",  ErrorMessage = "Invalid Email Format")]
        [StringLength(150, ErrorMessage = "Email must not exceed 150 characters")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; } = null!;

        [RegularExpression(
            @"^(\+62|62|0)8[1-9][0-9]{6,10}$",
            ErrorMessage = "Invalid phone number format"
        )]
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; }
        public Status Status { get; set; } = Status.Active;
    }


}
