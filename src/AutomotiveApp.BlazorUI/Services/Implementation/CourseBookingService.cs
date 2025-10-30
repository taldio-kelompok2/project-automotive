using System.Net;
using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class CourseBookingService(IHttpClientFactory httpFactory) : ICourseBookingService
    {
        private readonly HttpClient _http = httpFactory.CreateClient("ServerAPI");
        private const string BaseEndpoint = "api/CourseBooking";

        public async Task<ApiResponse<IEnumerable<CourseBookingQueryDto>>> GetUserBooking(Guid? courseId, CancellationToken ct = default)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<IEnumerable<CourseBookingQueryDto>>>($"{BaseEndpoint}/me?courseId={courseId}", ct)
            ?? new ApiResponse<IEnumerable<CourseBookingQueryDto>>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Data = [],
                Errors = ["Failed to load user Bookings"]
            };
            return response;
        }

        public async Task<ApiResponse<PaginatedResult<CourseBookingQueryDto>>> GetUserBookingPaged(Guid? courseId = null, int page = 1, int itemTaken = 6, CancellationToken ct = default)
        {
            var response = await _http.GetFromJsonAsync<ApiResponse<PaginatedResult<CourseBookingQueryDto>>>($"{BaseEndpoint}/me/paged?page={page}&itemTaken={itemTaken}&courseId={courseId}", ct)
            ?? new ApiResponse<PaginatedResult<CourseBookingQueryDto>>
            {
                Success = false,
                StatusCode = HttpStatusCode.BadRequest,
                Data = null,
                Errors = ["Failed to load user Bookings"]
            };

            return response;
        }
    }
}