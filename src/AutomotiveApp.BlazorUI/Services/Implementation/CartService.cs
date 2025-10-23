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
        private const string ItemEndpoint = "api/CartItems";

        public CartService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("ServerAPI");
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

            var response = await _http.GetAsync($"{BaseEndpoint}/me", cts.Token);

            var content = await response.Content.ReadAsStringAsync(ct);

            if (string.IsNullOrWhiteSpace(content))
            {
                return new ApiResponse<CartReadDetailsDto>
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Data = null,
                    Errors = ["Server returned empty response"]
                };
            }

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CartReadDetailsDto>>(cancellationToken: ct)
                ?? new ApiResponse<CartReadDetailsDto>
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Data = null,
                    Errors = ["Failed to parse server response."]
                };


            return result;

        }

        public async Task<ApiResponse<CartItemReadDto>> AddItemAsync(CartItemViewModel vm, CancellationToken ct = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(HttpTimeoutSeconds));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{ItemEndpoint}")
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