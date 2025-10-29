using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Shared.Dtos.Auth
{
    public class LoginResponseDto : BaseCommandDto, IDto
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; } = string.Empty;
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpiry { get; set; }
        public SignInResult? SignInResult { get; set; }
    }
}
