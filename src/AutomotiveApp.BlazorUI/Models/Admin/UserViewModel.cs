using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class UserViewModel
    {
        public Guid Id { get; set; }
        public  string Name { get; set; }
        public string Email { get; set; }
        public string? Password { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public Status Status { get; set; } = Status.Active;

    }
}
