using AutomotiveApp.BlazorUI.Models.Course;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Models;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface ITransactionService
    {
        Task<ApiResponse<string>> InstantPaymentAsync(Guid paymentId, Guid sessionId, CancellationToken ct = default);
        Task<ApiResponse<string>> CheckoutCartAsync(Guid cartId, Guid paymentMethodId, List<Guid> cartItemIds, CancellationToken ct = default);
    }
}