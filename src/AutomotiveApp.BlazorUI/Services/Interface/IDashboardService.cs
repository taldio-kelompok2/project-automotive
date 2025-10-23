using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetOverview();
        Task<List<DashboardUserDto>> GetDashboardTransactions();

    }
}