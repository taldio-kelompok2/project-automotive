using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IAuthService
    {
        Task<bool> LoginViaProxyAsync(string email, string password);
        Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<bool> LogoutViaProxyAsync();
        Task<bool> RefreshAuthProxyAsync();
        Task<UserProfileDto?> GetCurrentUserAsync();
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<bool> SendConfirmEmailAsync(SendConfirmEmailRequestDto sendConfirmEmailRequestDto);
        Task<bool> ConfirmEmailAsync(string userId, string token);
    }
}
