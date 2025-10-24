using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;


namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authStateProvider;
        public UserService(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authStateProvider = authStateProvider;
        }

        public async Task<IEnumerable<UserQueryDto>> GetAllUsers()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/user/all");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserQueryDto>>>();
                    return apiResponse?.Data;
                }
                else
                {
                    Console.WriteLine($"[UserService] Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<PaginatedResult<UserQueryDto>> GetPagedUsers(int page = 1, int itemTaken = 10, string? search = null)
        {
            try
            {
                //// Get token from local storage
                //var token = await _localStorage.GetItemAsync<string>("authToken");
                //if (!string.IsNullOrEmpty(token))
                //{
                //    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                //}

                var url = $"api/user/paged?page={page}&itemTaken={itemTaken}";

                if (!string.IsNullOrWhiteSpace(search))
                {
                    url += $"search={Uri.EscapeDataString(search)}";
                }

                Console.WriteLine($"[UserService] Fetching users from: {url}");

                var response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<PaginatedResult<UserQueryDto>>>();
                    return apiResponse?.Data;
                }
                else
                {
                    Console.WriteLine($"[UserService] Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateUser(UserCreateRequestDto userCreateDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/user", userCreateDto);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                    return apiResponse.Success;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] Create user exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateUser(Guid userId, UserUpdateRequestDto userUpdateDto)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/user/{userId}", userUpdateDto);

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                    return apiResponse.Success;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] Update user exception: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteUser(Guid userId)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/user/{userId}");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();
                    return apiResponse.Success;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] Delete user exception: {ex.Message}");
                return false;
            }
        }
    }
}