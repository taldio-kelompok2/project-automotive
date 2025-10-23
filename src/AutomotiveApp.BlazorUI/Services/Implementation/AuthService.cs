using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Response;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Net.Http.Headers;
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

        public async Task<bool> LoginViaProxyAsync(string email, string password)
        {
            var payload = new { Email = email, Password = password };
            var json = JsonSerializer.Serialize(payload);

            //JS Runtime
            return await _js.InvokeAsync<bool>(
                "loginViaFetch",
                "auth/proxy-login",
                json
            );
        }

        public async Task<bool> RefreshAuthProxyAsync()
        {
            //JS Runtime
            return await _js.InvokeAsync<bool>(
                "refreshViaFetch",
                "auth/proxy-refresh-token"
            );
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

                if (!response.IsSuccessStatusCode)
                    return null;

                var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();
                var data = apiResponse?.Data;

                if (data == null || apiResponse?.Success != true)
                    return null;

                var token = data.AccessToken;
                ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(token);

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Register error: {ex}");
                return null;
            }
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
