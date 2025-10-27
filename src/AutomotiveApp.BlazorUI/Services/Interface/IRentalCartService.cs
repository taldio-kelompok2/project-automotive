using AutomotiveApp.BlazorUI.Models;
using AutomotiveApp.BlazorUI.Models.Cart;
using AutomotiveApp.BlazorUI.Models.Transaction;
using AutomotiveApp.Shared.Dtos.CartItems;
using AutomotiveApp.Shared.Response;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IRentalCartService
    {
        List<RentalCartItemViewModel> RentalCartItems { get; set; }
        Guid Id { get; set; }
        bool IsLoading { get; }
        bool SelectedAll { get; set; }
        int TotalPrice { get; set; }
        bool IsCartEmpty { get; }
        event Action? OnCartChanged;
        event Action? OnLoadingChanged;
        Task GetUserCartData();
        void ToggleSelectAll(bool value);
        Task<ApiResponse<CartItemReadDto>> AddItem(Guid sessionid);
        Task<bool> RemoveSelectedItemsAsync();
        Task<bool> RemoveItemAsync(Guid sessionId);
        Task ClearAllAsync();
        Task FinalizeInstantPaymentAsync(Guid SessionId, Guid PaymentId);
        Task FinalizePaymentAsync(Guid PaymentId);
    }
}