using System.Threading;
using Microsoft.AspNetCore.Components.Forms;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

public interface ICourseCategoryService
{
    Task<IEnumerable<CourseCategoryQueryDto>> GetAllAsync(CancellationToken ct = default);

    Task<Guid?> CreateMultipartAsync(
        string name,
        string description,
        IBrowserFile? file = null,
        CancellationToken ct = default);

    Task<bool> UpdateMultipartAsync(
        Guid id,
        string name,
        string description,
        IBrowserFile? file = null,
        CancellationToken ct = default);

    Task<ApiResponse<PaginatedResult<CourseCategoryQueryDto>>> GetPagedAsync(
        int page,
        int pageSize = 8,
        CancellationToken ct = default);
}
