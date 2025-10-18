using System.Net.Http.Json;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

public class CourseCategoryService : ICourseCategoryService
{
    private readonly HttpClient _http;
    public CourseCategoryService(HttpClient http) => _http = http;

    public async Task<IEnumerable<CourseCategoryQueryDto>> GetAllAsync(CancellationToken ct = default)
    {
        var resp = await _http.GetFromJsonAsync<ApiResponse<IEnumerable<CourseCategoryQueryDto>>>(
            "api/CourseCategory", ct);
        return resp?.Data ?? Enumerable.Empty<CourseCategoryQueryDto>();
    }

    public async Task<Guid?> CreateMultipartAsync(
        string name,
        string description,
        IBrowserFile? file = null,
        CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();
        content.Add(new StringContent(name, Encoding.UTF8), "Name");
        content.Add(new StringContent(description, Encoding.UTF8), "Description");

        if (file is not null)
        {
            var stream = file.OpenReadStream(long.MaxValue);
            content.Add(new StreamContent(stream), "Image", file.Name);
        }

        using var resp = await _http.PostAsync("api/CourseCategory", content, ct);
        if (!resp.IsSuccessStatusCode) return null;

        var body = await resp.Content.ReadFromJsonAsync<ApiResponse<CourseCategoryQueryDto>>(cancellationToken: ct);
        return body?.Data?.Id;
    }

    public async Task<bool> UpdateMultipartAsync(
        Guid id,
        string name,
        string description,
        IBrowserFile? file = null,
        CancellationToken ct = default)
    {
        using var content = new MultipartFormDataContent();

        if (!string.IsNullOrWhiteSpace(name))
            content.Add(new StringContent(name, Encoding.UTF8), "Name");
        if (!string.IsNullOrWhiteSpace(description))
            content.Add(new StringContent(description, Encoding.UTF8), "Description");

        if (file is not null)
        {
            var stream = file.OpenReadStream(long.MaxValue);
            content.Add(new StreamContent(stream), "Image", file.Name);
        }

        using var req = new HttpRequestMessage(HttpMethod.Patch, $"api/CourseCategory/{id}")
        {
            Content = content
        };
        using var resp = await _http.SendAsync(req, ct);
        return resp.IsSuccessStatusCode;
    }

    public async Task<ApiResponse<PaginatedResult<CourseCategoryQueryDto>>> GetPagedAsync(
        int page, int pageSize = 8, CancellationToken ct = default)
        => await _http.GetFromJsonAsync<ApiResponse<PaginatedResult<CourseCategoryQueryDto>>>(
               $"api/CourseCategory/paged?page={page}&itemTaken={pageSize}", ct)
           ?? new ApiResponse<PaginatedResult<CourseCategoryQueryDto>>();
}
