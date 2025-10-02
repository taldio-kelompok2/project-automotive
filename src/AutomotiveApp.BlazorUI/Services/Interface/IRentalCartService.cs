using AutomotiveApp.BlazorUI.Models;
using AutomotiveApp.BlazorUI.Models.Cart;
using AutomotiveApp.BlazorUI.Models.Transaction;

namespace AutomotiveApp.BlazorUI.Services.Interface
{
    public interface IRentalCartService
    {
        List<RentalCartViewModel> RentalCart { get; set; }
        bool IsLoading { get; }
        bool SelectedAll { get; set; }
        int TotalPrice { get; set; }
        bool IsCartEmpty { get; }
        event Action? OnCartChanged;
        event Action? OnLoadingChanged;
        void ToggleSelectAll(bool value);
        void AddItem(CarViewModel cartItem, DateTime? rentalDate);
        bool TryRemoveItem(RentalCartViewModel cartItem);
        void ClearAll();
        Task FinalizeInstantPaymentAsync(CarViewModel rental, TransactionViewModel transaction, DateTime? rentalDate);
        Task FinalizePaymentAsync();
    }
}