using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class UserViewModel
    {
        public  string Email { get; set; }
        public  string Name { get; set; }
        public  UserRole Role { get; set; }
        public Status Status { get; set; } = Status.Active; // default

    }
}
