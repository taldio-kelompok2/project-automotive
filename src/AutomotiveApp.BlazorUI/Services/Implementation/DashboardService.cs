using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Response;
using Microsoft.AspNetCore.Components.Authorization;


namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authStateProvider;
        public DashboardService(
            HttpClient httpClient,
            AuthenticationStateProvider authStateProvider)
        {
            _httpClient = httpClient;
            _authStateProvider = authStateProvider;
        }

        public async Task<List<DashboardUserDto>> GetDashboardTransactions()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/dashboard/transactions");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<DashboardUserDto>>>();
                    return apiResponse?.Data;
                }
                else
                {
                    Console.WriteLine($"[DashboardService] Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] Exception: {ex.Message}");
                return null;
            }
        }

        public async Task<DashboardDto> GetOverview()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/dashboard/overview");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<DashboardDto>>();
                    return apiResponse?.Data;
                }
                else
                {
                    Console.WriteLine($"[DashboardService] Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] Exception: {ex.Message}");
                return null;
            }
        }
    }
}