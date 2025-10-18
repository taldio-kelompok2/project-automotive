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

        public TransactionService(HttpClient http)
        {
            _http = http;
            var dummyToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOiJhZWFmYjY3MS05NDIzLTQ2MTMtODkxMC1hYmVkY2ZiNDg0ODUiLCJlbWFpbCI6ImJ1eWVyQGV4YW1wbGUuY29tIiwidW5pcXVlX25hbWUiOiJidXllckBleGFtcGxlLmNvbSIsImp0aSI6ImNmY2JmM2U2LWE3NzktNDk0ZC04ODk2LTdiNjUwOWJlZjQ5OSIsImlhdCI6MTc2MDYxNDc2MSwicm9sZSI6IkJ1eWVyIiwibmJmIjoxNzYwNjE0NzYxLCJleHAiOjE3NjA2MTgzNjEsImlzcyI6IkF1dG9tb3RpdmVBcHAiLCJhdWQiOiJBdXRvbW90aXZlQXBwLVVzZXJzIn0.HPFJlMGz1dPbl-yYARfGdt24r68CJ7PR48jzqcWcekk";
            _http.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", dummyToken);
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