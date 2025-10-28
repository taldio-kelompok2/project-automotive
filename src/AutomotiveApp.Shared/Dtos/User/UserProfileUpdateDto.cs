using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomotiveApp.Shared.Dtos.User
{
    public class UserProfileUpdateDto : BaseCommandDto, IDto
    {
        public Guid CurrentUserId { get; set; }
        [StringLength(50, MinimumLength = 4, ErrorMessage = "Username must be between 4-50 characters")]
        public string? UserName { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email")]
        public string? Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Invalid phone number format")]
        public string? PhoneNumber { get; set; }
    }
}
