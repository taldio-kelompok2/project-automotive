using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.Forms;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface;

public interface ICourseService
{
    Task<ApiResponse<CourseQueryDto>> CreateMultipartAsync(string name, string description, int price, Guid categoryId, IBrowserFile? file = null, CancellationToken ct = default);
    Task<bool> UpdateMultipartAsync(Guid id, string? name, string? description, int? price, Guid? categoryId, IBrowserFile? file = null, CancellationToken ct = default);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct = default);

    Task<ApiResponse<IEnumerable<CourseQueryDto>>> GetAllAsync(CancellationToken ct = default);
    Task<ApiResponse<CourseQueryDetailDto>> GetById(Guid id, CancellationToken ct = default);
    Task<ApiResponse<PaginatedResult<CourseQueryDto>>> GetPaged(int Page = 1, int ItemTaken = 6, bool isRandom = false, CancellationToken ct = default);
}
