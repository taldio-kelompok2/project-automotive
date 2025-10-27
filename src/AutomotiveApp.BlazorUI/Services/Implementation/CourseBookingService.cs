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
                StatusCode = HttpStatusCode.InternalServerError,
                Data = [],
                Errors = ["Failed to load user Bookings"]
            };

            return response;
        }
    }
}