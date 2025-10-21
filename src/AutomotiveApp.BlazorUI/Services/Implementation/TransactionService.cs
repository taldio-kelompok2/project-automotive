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
    public class TransactionService : ITransactionService
    {
        private readonly HttpClient _http;
        private const string BaseEndpoint = "api/orders";

        public TransactionService(IHttpClientFactory httpClientFactory)
        {
            _http = httpClientFactory.CreateClient("ServerAPI");
        }

        private const int HttpTimeoutSeconds = 10;

        public async Task<ApiResponse<string>> CheckoutCartAsync(
            Guid cartId,
            Guid paymentMethodId,
            List<Guid> cartItemIds,
            CancellationToken ct = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(HttpTimeoutSeconds));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseEndpoint}")
            {
                Content = JsonContent.Create(new
                {
                    CartId = cartId,
                    PaymentMethodId = paymentMethodId,
                    CartItemIds = cartItemIds
                })
            };

            var response = await _http.SendAsync(request, cts.Token);

            return await response.Content.ReadFromJsonAsync<ApiResponse<string>>(cts.Token)
                ?? new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Data = null,
                    Errors = ["No response from server"]
                };
        }

        public async Task<ApiResponse<string>> InstantPaymentAsync(Guid paymentId, Guid sessionId,
            CancellationToken ct = default)
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(ct);
            cts.CancelAfter(TimeSpan.FromSeconds(HttpTimeoutSeconds));

            var request = new HttpRequestMessage(HttpMethod.Post, $"{BaseEndpoint}/instant")
            {
                Content = JsonContent.Create(new
                {
                    PaymentId = paymentId,
                    SessionId = sessionId
                })
            };

            var response = await _http.SendAsync(request, cts.Token);

            return await response.Content.ReadFromJsonAsync<ApiResponse<string>>(cts.Token)
                ?? new ApiResponse<string>
                {
                    Success = false,
                    StatusCode = response.StatusCode,
                    Data = null,
                    Errors = ["No response from server"]
                };
        }
    }
}