using System.Net.Http.Json;
using System.Text; // Encoding
using Microsoft.AspNetCore.Components.Forms;
using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;
using System.Net;
using AutomotiveApp.Shared.Config;

namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class CourseService(IHttpClientFactory httpFactory) : ICourseService

    {
        private readonly HttpClient _http = httpFactory.CreateClient("ServerAPI");
        private const string BaseEndpoint = "api/Course";

        public async Task<ApiResponse<CourseQueryDto>> CreateMultipartAsync(
            string name,
            string description,
            int price,
            Guid categoryId,
            IBrowserFile? file = null,
            CancellationToken ct = default)
        {
            using var content = new MultipartFormDataContent
            {
                { new StringContent(name, Encoding.UTF8), "Name" },
                { new StringContent(description, Encoding.UTF8), "Description" },
                { new StringContent(price.ToString(), Encoding.UTF8), "Price" },
                { new StringContent(categoryId.ToString(), Encoding.UTF8), "CategoryId" }
            };

            if (file is not null)
            {
                var stream = file.OpenReadStream(FileUploadConfig.MaxFileSize, ct);
                content.Add(new StreamContent(stream), "Image", file.Name);
            }

            var response = await _http.PostAsync($"{BaseEndpoint}", content, ct);

            var body = await response.Content.ReadFromJsonAsync<ApiResponse<CourseQueryDto>>(cancellationToken: ct)
            ?? new ApiResponse<CourseQueryDto>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
                Errors = ["Failed to parse server response"]
            };

            return body;
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
                var stream = file.OpenReadStream(FileUploadConfig.MaxFileSize, ct);
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
            var response = await _http.GetAsync($"{BaseEndpoint}/{id}", ct);

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<CourseQueryDetailDto>
                {
                    Success = false,
                    Data = null,
                    Errors = new[] { $"Request failed: {response.StatusCode}" }
                };

            var data = await response.Content.ReadFromJsonAsync<ApiResponse<CourseQueryDetailDto>>(cancellationToken: ct);

            return data ?? new ApiResponse<CourseQueryDetailDto> { Success = false, Data = null };
            ;
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
