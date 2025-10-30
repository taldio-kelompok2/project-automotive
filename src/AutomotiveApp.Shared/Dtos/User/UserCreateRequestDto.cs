using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.User
{
    public class UserCreateRequestDto : BaseCommandDto, IDto
    {
        public required string UserName { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public required string? PhoneNumber { get; set; }
        public required string Role { get; set; }
        public required bool Status { get; set; } = true;
    }
}
