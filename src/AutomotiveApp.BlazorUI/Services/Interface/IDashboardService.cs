using AutomotiveApp.Shared.Dtos.Auth;
using AutomotiveApp.Shared.Dtos.User;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.WebAPI.Dto.Courses;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetOverview();
        Task<List<CourseStatisticDto>> GetCourseStatistics(int? year);
        Task<List<DashboardTransactionDto>> GetDashboardTransactions();

    }
}