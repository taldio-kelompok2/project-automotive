using System.Net;
using AutomotiveApp.BlazorUI.Models.Course;
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

        public Task<Guid?> CreateAsync(CreateCourseViewModel vm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(Guid id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<IEnumerable<CourseQueryDto>>> GetAllAsync(CancellationToken ct = default)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<IEnumerable<CourseQueryDto>>>($"{BaseEndpoint}", ct)
            ?? new ApiResponse<IEnumerable<CourseQueryDto>>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = Array.Empty<CourseQueryDto>(),
                Errors = ["Failed to load courses"]
            };

            return response;
        }

        public async Task<ApiResponse<CourseQueryDetailDto>> GetById(Guid id, CancellationToken ct = default)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<CourseQueryDetailDto>>($"{BaseEndpoint}/{id}", ct)
            ?? new ApiResponse<CourseQueryDetailDto>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
                Errors = ["Failed to load courses"]
            };

            return response;
        }

        public async Task<ApiResponse<PaginatedResult<CourseQueryDto>>> GetPaged(int Page = 1, int ItemTaken = 6, bool isRandom = false, CancellationToken ct = default)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<PaginatedResult<CourseQueryDto>>>($"{BaseEndpoint}/paged/?page={Page}&itemTaken={ItemTaken}&isRandom={isRandom}", ct)
            ?? new ApiResponse<PaginatedResult<CourseQueryDto>>
            {
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Data = null,
                Errors = ["Failed to load courses"]
            };

            return response;
        }


        public Task<bool> UpdateAsync(Guid id, UpdateCourseViewModel vm, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}