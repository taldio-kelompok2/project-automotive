using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface ICourseBookingService
    {
        Task<ApiResponse<IEnumerable<CourseBookingQueryDto>>> GetUserBooking(Guid? CourseId = null, CancellationToken ct = default);
    }
}