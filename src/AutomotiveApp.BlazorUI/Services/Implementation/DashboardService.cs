using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Response;
using Microsoft.AspNetCore.Components.Authorization;


namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;
        public DashboardService(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("ServerAPI");
        }

        public async Task<List<DashboardTransactionDto>> GetDashboardTransactions()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/dashboard/transactions");

                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponse<List<DashboardTransactionDto>>>();
                    return apiResponse?.Data ?? [];
                }
                else
                {
                    Console.WriteLine($"[DashboardService] Error: {response.StatusCode} - {await response.Content.ReadAsStringAsync()}");
                    return [];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[DashboardService] Exception: {ex.Message}");
                return [];
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