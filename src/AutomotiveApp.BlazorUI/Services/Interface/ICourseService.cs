using AutomotiveApp.BlazorUI.Models.Course;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface ICourseService
    {
        Task<Guid?> CreateAsync(CreateCourseViewModel vm, CancellationToken ct = default);
        Task<bool> UpdateAsync(Guid id, UpdateCourseViewModel vm, CancellationToken ct = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
        Task<ApiResponse<IEnumerable<CourseQueryDto>>> GetAllAsync(CancellationToken ct = default);
        Task<ApiResponse<CourseQueryDetailDto>> GetById(Guid id, CancellationToken ct = default);
        Task<ApiResponse<PaginatedResult<CourseQueryDto>>> GetPaged(int Page = 1, int ItemTaken = 6, bool isRandom = false, CancellationToken ct = default);
    }
}