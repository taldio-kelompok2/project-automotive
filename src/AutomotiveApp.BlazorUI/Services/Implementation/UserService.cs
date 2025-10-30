using AutomotiveApp.BlazorUI.Components.Pages;
using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;


namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        public UserService(
            IHttpClientFactory httpFactory)
        {
            _httpClient = httpFactory.CreateClient("ServerAPI");
        }

        public async Task<IEnumerable<UserQueryDto>> GetAllUsers()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/user/all");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<UserQueryDto>>>();

                    return apiResponse?.Data ?? [];
                }
                else
                {
                    Console.WriteLine($"[UserService] Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return [];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] Exception: {ex.Message}");
                return [];
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
                    return apiResponse?.Data ?? new PaginatedResult<UserQueryDto>([], 0);
                }
                else
                {
                    Console.WriteLine($"[UserService] Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return new PaginatedResult<UserQueryDto>([], 0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[UserService] Exception: {ex.Message}");
                return new PaginatedResult<UserQueryDto>([], 0);
            }
        }

        public async Task<ApiResponse<UserCreateRequestDto>> CreateUser(UserCreateRequestDto userCreateDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/user", userCreateDto);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<UserCreateRequestDto>>()
            ?? new ApiResponse<UserCreateRequestDto>()
            {
                Success = false,
                Data = null,
                Errors = ["Something went wrong with the request"]
            };
            return apiResponse;
        }

        public async Task<ApiResponse<bool>> UpdateUser(Guid userId, UserUpdateRequestDto userUpdateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/user/{userId}", userUpdateDto);
            var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<bool>>()
            ?? new ApiResponse<bool>()
            {
                Success = false,
                Data = false,
                Errors = ["Something went wrong with the request"]
            };

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
                    return apiResponse?.Success ?? false;
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