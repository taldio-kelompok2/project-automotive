using System.ComponentModel.DataAnnotations;
using System.Net;
using AutomotiveApp.BlazorUI.Models.Cart;
using AutomotiveApp.BlazorUI.Models.Course;
using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.CartItems;
using AutomotiveApp.Shared.Dtos.Carts;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class CartService : ICartService
    {
        private readonly HttpClient _http;
        private const string BaseEndpoint = "api/Carts";
        private const string AddCartEndpoint = "api/CartItems";

        public CartService(HttpClient http)
        {
            _http = http;
            var dummyToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJhZWFmYjY3MS05NDIzLTQ2MTMtODkxMC1hYmVkY2ZiNDg0ODUiLCJlbWFpbCI6ImJ1eWVyQGV4YW1wbGUuY29tIiwidW5pcXVlX25hbWUiOiJidXllckBleGFtcGxlLmNvbSIsImp0aSI6IjYxNDQ2MzI4LTA2M2UtNDYxYS1hMTZlLWRhZjE0ZWEwMzM2ZSIsImlhdCI6MTc2MDU5MTM2OSwicm9sZSI6IkJ1eWVyIiwibmJmIjoxNzYwNTkxMzY5LCJleHAiOjE3NjA1OTQ5NjksImlzcyI6IkF1dG9tb3RpdmVBcHAiLCJhdWQiOiJBdXRvbW90aXZlQXBwLVVzZXJzIn0.f2Ie2al2Bpt6JXJ2f6CBIhC7on0dZT1w69Pm0phuhys";
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dummyToken);
        }

        private const int HttpTimeoutSeconds = 10;

        public async Task<ApiResponse<CartReadDetailsDto>> CreateAsync(Guid userId, CancellationToken ct = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(HttpTimeoutSeconds));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseEndpoint}")
            {
                Content = JsonContent.Create(new { UserId = userId })
            };

            var response = await _http.SendAsync(request, cts.Token);

            return await response.Content.ReadFromJsonAsync<ApiResponse<CartReadDetailsDto>>(cancellationToken: cts.Token)
                ?? new ApiResponse<CartReadDetailsDto>
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Data = null,
                    Errors = ["No response data."]
                };
        }

        public async Task<ApiResponse<CartReadDetailsDto>> GetUserCart(CancellationToken ct = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(HttpTimeoutSeconds));

            var response = await _http.GetFromJsonAsync<ApiResponse<CartReadDetailsDto>>($"{BaseEndpoint}/me", cts.Token)
                            ?? new ApiResponse<CartReadDetailsDto>
                            {
                                Success = false,
                                StatusCode = HttpStatusCode.InternalServerError,
                                Data = null,
                                Errors = ["Failed to load courses"]
                            };

            return response;
        }

        public async Task<ApiResponse<CartItemReadDto>> AddItemAsync(CartItemViewModel vm, CancellationToken ct = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(HttpTimeoutSeconds));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{AddCartEndpoint}")
            {
                Content = JsonContent.Create(new { vm.CartId, vm.SessionId })
            };

            var response = await _http.SendAsync(request, cts.Token);

            return await response.Content.ReadFromJsonAsync<ApiResponse<CartItemReadDto>>(cancellationToken: cts.Token)
                ?? new ApiResponse<CartItemReadDto>
                {
                    Success = false,
                    StatusCode = HttpStatusCode.InternalServerError,
                    Data = null,
                    Errors = ["Failed to load courses"]
                };
        }

        public async Task<ApiResponse<string>> BatchDeleteItemsAsync(List<Guid> Ids, CancellationToken ct = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(HttpTimeoutSeconds));

            var request = new HttpRequestMessage(HttpMethod.Delete, $"{BaseEndpoint}/items")
            {
                Content = JsonContent.Create(Ids)
            };

            var response = await _http.SendAsync(request, cts.Token);

            return await response.Content.ReadFromJsonAsync<ApiResponse<string>>(cancellationToken: cts.Token)
                ?? new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Data = null,
                    Errors = ["No response data."]
                };
        }
    }
}