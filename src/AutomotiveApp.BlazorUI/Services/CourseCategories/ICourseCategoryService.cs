using System.Threading;
using AutomotiveApp.Shared.Models;         
using AutomotiveApp.Shared.Dtos.Courses;    
using Microsoft.AspNetCore.Components.Forms;

public interface ICourseCategoryService
{
    Task<IEnumerable<CourseCategoryQueryDto>> GetAllAsync(CancellationToken ct = default);
    Task<Guid?> CreateMultipartAsync(string name, string description, IBrowserFile file, CancellationToken ct = default);
    Task<bool> UpdateMultipartAsync(Guid id, string name, string description, IBrowserFile? file = null, CancellationToken ct = default);
    Task<PaginatedResult<CourseCategoryQueryDto>?> GetPagedAsync(int page = 1, int itemTaken = 6, CancellationToken ct = default);
}