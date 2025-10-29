using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Response;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Text.Json;


namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _js;
        private readonly AuthenticationStateProvider _authStateProvider;
        public AuthService(
            IHttpClientFactory httpFactory,
            IJSRuntime jSRuntime,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpFactory.CreateClient("ServerAPI");
            _js = jSRuntime;
            _authStateProvider = authStateProvider;
        }

        public async Task CheckAuthAsync()
        {
            await _httpClient.GetAsync("/api/auth/check");
        }

        public async Task<LoginFrontendResponseDto> LoginViaProxyAsync(string email, string password)
        {
            var payload = new { Email = email, Password = password };
            var json = JsonSerializer.Serialize(payload);

            //JS Runtime
            var result = await _js.InvokeAsync<JsonElement>(
                "loginViaFetch",
                "auth/proxy-login",
                json
            );

            var success = result.GetProperty("ok").GetBoolean();
            var response = new LoginFrontendResponseDto
            {
                Success = success,
                Status = result.GetProperty("status").GetInt32()
            };

            if (!success)
            {
                var data = result.GetProperty("data");
                if (data.TryGetProperty("Errors", out var errors))
                {
                    response.Message = errors[0].GetString();
                }
            }
            else
            {
                response.Message = "Login successful!";
            }

            return response;
        }

        public async Task<bool> RefreshAuthProxyAsync()
        {
            //JS Runtime
            return await _js.InvokeAsync<bool>(
                "refreshViaFetch",
                "auth/proxy-refresh-token"
            );
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
            var data = apiResponse.Data;

            if (!apiResponse.Success)
            {
                data.Message = apiResponse.Errors.FirstOrDefault();
            }

            return data;
        }

        public async Task<bool> LogoutViaProxyAsync()
        {
            _httpClient.DefaultRequestHeaders.Authorization = null;

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();

            return await _js.InvokeAsync<bool>(
                "logoutViaFetch",
                "auth/proxy-logout"
            );
        }

        public async Task<UserProfileDto?> GetCurrentUserAsync()
        {
            try
            {

                var response = await _httpClient.GetAsync("api/user/me");

                if (!response.IsSuccessStatusCode)
                    return null;

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserProfileDto>>();
                return apiResponse?.Data;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ApiResponse<UserProfileUpdateDto>> UpdateProfileAsync(UserProfileUpdateDto request)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/me", request);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserProfileUpdateDto>>();

            if (!response.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success)
            {
                var message = apiResponse?.Errors?.FirstOrDefault() ?? "Internal error";
                throw new Exception(message);
            }
            return apiResponse;
        }

        // Forgot/Reset/Confirm Email methods remain unchanged
        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/reset-password", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendConfirmEmailAsync(SendConfirmEmailRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/send-confirm-email", request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ConfirmEmailAsync(string userId, string token)
        {
            try
            {
                var url = $"api/auth/confirm-email?userId={Uri.EscapeDataString(userId)}&token={Uri.EscapeDataString(token)}";
                var response = await _httpClient.PostAsync(url, null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
