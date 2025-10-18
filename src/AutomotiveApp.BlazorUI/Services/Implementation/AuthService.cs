using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Response;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using MyApp.BlazorUI.Services;
using System.Net.Http.Headers;


namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        public AuthService(
            IHttpClientFactory httpFactory,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpFactory.CreateClient("ServerAPI");
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }


        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto request)
        {
            Console.WriteLine($"login start:{request.Email} {request.Password}");
            try
            {
                var cts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                Console.WriteLine($"{_httpClient.BaseAddress}");

                var response = await _httpClient.PostAsJsonAsync("api/auth/login", request, cts.Token);

                Console.WriteLine($"response: {response.StatusCode} {response.Content}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        await _localStorage.SetItemAsStringAsync("authToken", apiResponse.Data.AccessToken);
                        await _localStorage.SetItemAsStringAsync("refreshToken", apiResponse.Data.RefreshToken);

                        _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", apiResponse.Data.AccessToken);

                        ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(apiResponse.Data.AccessToken);

                        return apiResponse.Data;
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"err: {ex.Message}");
                return null;
            }
        }

        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", request);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<AuthResponseDto>>();

                    if (apiResponse?.Success == true && apiResponse.Data != null)
                    {
                        await _localStorage.SetItemAsStringAsync("authToken", apiResponse.Data.AccessToken);
                        await _localStorage.SetItemAsStringAsync("refreshToken", apiResponse.Data.RefreshToken);

                        _httpClient.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", apiResponse.Data.AccessToken);

                        ((CustomAuthStateProvider)_authStateProvider).NotifyUserAuthentication(apiResponse.Data.AccessToken);

                        return apiResponse.Data;
                    }
                }

                return null;
            }
            catch
            {
                return null;
            }
        }
        public async Task LogoutAsync()
        {
            await _localStorage.RemoveItemAsync("authToken");
            await _localStorage.RemoveItemAsync("refreshToken");

            _httpClient.DefaultRequestHeaders.Authorization = null;

            ((CustomAuthStateProvider)_authStateProvider).NotifyUserLogout();
        }
        public async Task<UserProfileDto?> GetCurrentUserAsync(string token)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                Console.WriteLine($"[authservice] auth: {_httpClient.DefaultRequestHeaders.Authorization}");
                var response = await _httpClient.GetAsync("api/user/me");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserProfileDto>>();
                    return apiResponse?.Data;
                }

                return null;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> ForgotPasswordAsync(ForgotPasswordRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/forgot-password", request);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
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

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
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

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
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
