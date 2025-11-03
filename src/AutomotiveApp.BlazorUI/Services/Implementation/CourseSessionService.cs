using System.Net.Http.Json;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.CourseSessions;

public class CourseSessionService(HttpClient http) : ICourseSessionService
{
    private readonly HttpClient _http = http;
    private const string BaseEndpoint = "api/CourseSession";

    public async Task<Guid?> CreateAsync(CourseSessionCommandDto dto, CancellationToken ct = default)
    {
        var resp = await _http.PostAsJsonAsync(BaseEndpoint, dto, ct);
        if (!resp.IsSuccessStatusCode) return null;

        return dto.Id;
    }

    public async Task<bool> UpdateAsync(Guid id, CourseSessionEditCommandDto dto, CancellationToken ct = default)
        => (await _http.PutAsJsonAsync($"{BaseEndpoint}/{id}", dto, ct)).IsSuccessStatusCode;

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        => (await _http.DeleteAsync($"{BaseEndpoint}/{id}", ct)).IsSuccessStatusCode;

    public async Task<ApiResponse<IEnumerable<CourseSessionQueryDto>>> GetAllAsync(CancellationToken ct = default)
        => await _http.GetFromJsonAsync<ApiResponse<IEnumerable<CourseSessionQueryDto>>>(BaseEndpoint, ct)
           ?? new ApiResponse<IEnumerable<CourseSessionQueryDto>> { Data = Enumerable.Empty<CourseSessionQueryDto>() };
}