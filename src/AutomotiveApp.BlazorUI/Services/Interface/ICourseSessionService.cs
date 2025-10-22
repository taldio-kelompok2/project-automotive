using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Response;

public interface ICourseSessionService
{
    Task<Guid?> CreateAsync(CourseSessionCommandDto dto, CancellationToken ct = default);
    Task<bool> UpdateAsync(Guid id, CourseSessionEditCommandDto dto, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);
    Task<ApiResponse<IEnumerable<CourseSessionQueryDto>>> GetAllAsync(CancellationToken ct = default);
}