using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task LogoutAsync();
        Task<UserProfileDto?> GetCurrentUserAsync(string token);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<bool> SendConfirmEmailAsync(SendConfirmEmailRequestDto sendConfirmEmailRequestDto);
        Task<bool> ConfirmEmailAsync(string userId, string token);
    }
}
