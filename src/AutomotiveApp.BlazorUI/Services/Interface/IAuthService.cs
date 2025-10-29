using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IAuthService
    {
        Task CheckAuthAsync();
        Task<LoginFrontendResponseDto> LoginViaProxyAsync(string email, string password);
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto registerRequestDto);
        Task<bool> LogoutViaProxyAsync();
        Task<bool> RefreshAuthProxyAsync();
        Task<UserProfileDto?> GetCurrentUserAsync();
        Task<ApiResponse<UserProfileUpdateDto>> UpdateProfileAsync(UserProfileUpdateDto request);
        Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto forgotPasswordRequestDto);
        Task<bool> ResetPasswordAsync(ResetPasswordRequestDto resetPasswordRequestDto);
        Task<bool> SendConfirmEmailAsync(SendConfirmEmailRequestDto sendConfirmEmailRequestDto);
        Task<bool> ConfirmEmailAsync(string userId, string token);
    }
}
