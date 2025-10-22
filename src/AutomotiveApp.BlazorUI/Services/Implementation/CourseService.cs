using System.Net.Http.Json;
using System.Text; // Encoding
using Microsoft.AspNetCore.Components.Forms;
using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class CourseService(HttpClient http) : ICourseService
    {
        private readonly HttpClient _http = http;
        private const string BaseEndpoint = "api/Course";

        public async Task<Guid?> CreateMultipartAsync(
            string name,
            string description,
            int price,
            Guid categoryId,
            IBrowserFile? file = null,
            CancellationToken ct = default)
        {
            using var content = new MultipartFormDataContent();
            content.Add(new StringContent(name, Encoding.UTF8), "Name");
            content.Add(new StringContent(description, Encoding.UTF8), "Description");
            content.Add(new StringContent(price.ToString(), Encoding.UTF8), "Price");
            content.Add(new StringContent(categoryId.ToString(), Encoding.UTF8), "CategoryId");

            if (file is not null)
            {
                var stream = file.OpenReadStream(long.MaxValue);
                content.Add(new StreamContent(stream), "Image", file.Name);
            }

            using var resp = await _http.PostAsync($"{BaseEndpoint}", content, ct);
            if (!resp.IsSuccessStatusCode) return null;

            var body = await resp.Content.ReadFromJsonAsync<ApiResponse<CourseQueryDto>>(cancellationToken: ct);
            return body?.Data?.Id;
        }

        public async Task<bool> UpdateMultipartAsync(
            Guid id,
            string? name,
            string? description,
            int? price,
            Guid? categoryId,
            IBrowserFile? file = null,
            CancellationToken ct = default)
        {
            using var content = new MultipartFormDataContent();

            if (!string.IsNullOrWhiteSpace(name))
                content.Add(new StringContent(name, Encoding.UTF8), "Name");
            if (!string.IsNullOrWhiteSpace(description))
                content.Add(new StringContent(description, Encoding.UTF8), "Description");
            if (price.HasValue)
                content.Add(new StringContent(price.Value.ToString(), Encoding.UTF8), "Price");
            if (categoryId.HasValue)
                content.Add(new StringContent(categoryId.Value.ToString(), Encoding.UTF8), "CategoryId");

            if (file is not null)
            {
                var stream = file.OpenReadStream(long.MaxValue);
                content.Add(new StreamContent(stream), "Image", file.Name);
            }

            using var req = new HttpRequestMessage(HttpMethod.Patch, $"{BaseEndpoint}/{id}")
            {
                Content = content
            };
            using var resp = await _http.SendAsync(req, ct);
            return resp.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            var resp = await _http.DeleteAsync($"{BaseEndpoint}/{id}", ct);
            return resp.IsSuccessStatusCode;
        }

        public async Task<ApiResponse<IEnumerable<CourseQueryDto>>> GetAllAsync(CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<ApiResponse<IEnumerable<CourseQueryDto>>>($"{BaseEndpoint}", ct)
                   ?? new ApiResponse<IEnumerable<CourseQueryDto>> { Success = false, Data = Array.Empty<CourseQueryDto>() };
        }

        public async Task<ApiResponse<CourseQueryDetailDto>> GetById(Guid id, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<ApiResponse<CourseQueryDetailDto>>($"{BaseEndpoint}/{id}", ct)
                   ?? new ApiResponse<CourseQueryDetailDto> { Success = false, Data = null };
        }

        public async Task<ApiResponse<PaginatedResult<CourseQueryDto>>> GetPaged(
            int Page = 1, int ItemTaken = 6, bool isRandom = false, CancellationToken ct = default)
        {
            return await _http.GetFromJsonAsync<ApiResponse<PaginatedResult<CourseQueryDto>>>(
                       $"{BaseEndpoint}/paged?page={Page}&itemTaken={ItemTaken}&isRandom={isRandom}", ct)
                   ?? new ApiResponse<PaginatedResult<CourseQueryDto>> { Success = false, Data = null };
        }
    }
}
