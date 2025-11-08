using AutomotiveApp.Shared.Dtos.Analytics;
using AutomotiveApp.Shared.Response;
using System.Net.Http.Json;

namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public interface IAnalyticsService
    {
        Task<ApiResponse<AnalyticsOverviewDto>> GetOverviewAsync();
    }

    public class AnalyticsService(HttpClient http) : IAnalyticsService
    {
        public async Task<ApiResponse<AnalyticsOverviewDto>> GetOverviewAsync()
        {
            var res = await http.GetFromJsonAsync<ApiResponse<AnalyticsOverviewDto>>("api/analytics/overview");
            return res ?? new ApiResponse<AnalyticsOverviewDto> { Success = false };
        }
    }
}
