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
        public UserService(
            HttpClient httpClient)
        {
            _httpClient = httpClient;
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

        public async Task<ApiResponse<bool>> CreateUser(UserCreateRequestDto userCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user", userCreateDto);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (!response.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success)
            {
                var message = apiResponse?.Errors?.FirstOrDefault() ?? "Internal error";
                Console.WriteLine($"create user: {message}");
                throw new Exception(message);
            }
            return apiResponse;
        }

        public async Task<ApiResponse<bool>> UpdateUser(Guid userId, UserUpdateRequestDto userUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/{userId}", userUpdateDto);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>();

            if (!response.IsSuccessStatusCode || apiResponse == null || !apiResponse.Success)
            {
                var message = apiResponse?.Errors?.FirstOrDefault() ?? "Internal error";
                throw new Exception(message);
            }
            return apiResponse;
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