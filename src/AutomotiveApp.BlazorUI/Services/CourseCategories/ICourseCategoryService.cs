using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.CourseCategories;
public interface ICourseCategoryService
{
    Task<IEnumerable<CourseCategoryQueryDto>> GetAllAsync(CancellationToken ct = default);

    Task<Guid?> CreateMultipartAsync(
        string name,
        string description,
        IBrowserFile? file = null,
        IBrowserFile? heroFile = null,
        CancellationToken ct = default);

    Task<bool> UpdateMultipartAsync(
        Guid id,
        string name,
        string description,
        IBrowserFile? file = null,
        IBrowserFile? heroFile = null,
        CancellationToken ct = default);

    Task<ApiResponse<PaginatedResult<CourseCategoryQueryDto>>> GetPagedAsync(
        int page,
        int pageSize = 8,
        CancellationToken ct = default);
}
