using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface ICourseBookingService
    {
        Task<ApiResponse<IEnumerable<CourseBookingQueryDto>>> GetUserBooking(Guid? CourseId = null, CancellationToken ct = default);
        Task<ApiResponse<PaginatedResult<CourseBookingQueryDto>>> GetUserBookingPaged(Guid? courseId = null, int page = 1, int itemTaken = 6, CancellationToken ct = default);
    }
}