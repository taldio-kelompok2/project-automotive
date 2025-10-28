using AutomotiveApp.BlazorUI.Models.Cart;
using AutomotiveApp.BlazorUI.Services.Interface;
using AutomotiveApp.Shared.Dtos.CartItems;
using AutomotiveApp.Shared.Response;
using Microsoft.AspNetCore.Http.Features;
using MudBlazor;

namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class RentalCartService : IRentalCartService
    {
        private readonly ILogger<RentalCartService> _logger;
        private readonly ICartService _cartService;
        private readonly ITransactionService _transactionService;
        public Guid Id { get; set; }
        public List<RentalCartItemViewModel> RentalCartItems { get; set; } = [];
        public bool IsLoading { get; set; } = false;
        public bool IsCartEmpty => RentalCartItems.Count == 0;
        public bool SelectedAll
        {
            get => RentalCartItems?.All(c => c.Selected) ?? false;
            set
            {
                // When SelectedAll is set, update all individual items
                if (RentalCartItems != null)
                {
                    foreach (var item in RentalCartItems)
                    {
                        item.Selected = value;
                    }
                }
                // Notify subscribers that the cart changed
                OnCartChanged?.Invoke();
            }
        }
        public int TotalPrice => CalculateTotalPrice(); // Now calculated dynamically
        public event Action? OnCartChanged;
        public event Action? OnLoadingChanged;

        public RentalCartService(
            ILogger<RentalCartService> logger,
            ICartService cartService,
            ITransactionService transactionService
            )
        {
            _cartService = cartService;
            _logger = logger;
            _transactionService = transactionService;
        }

        public async Task GetUserCartData()
        {
            var cart = await _cartService.GetUserCart();
            _logger.LogInformation("Cart API Response: Success={Success}, Status={StatusCode}", cart.Success, cart.StatusCode);

            if (cart.Success && cart.Data != null)
            {
                Id = cart.Data.Id;
                var previousSelections = RentalCartItems
                    .Where(item => item.Selected)
                    .Select(item => item.Item.SessionId)
                    .ToHashSet() ?? new HashSet<Guid>();

                RentalCartItems.Clear();
                var newItems = cart.Data.Items.Select(item => new RentalCartItemViewModel
                {
                    Item = item,
                    Selected = previousSelections.Contains(item.SessionId)
                }).ToList();

                RentalCartItems.AddRange(newItems);
                _logger.LogInformation("Cart items count: {Count}", cart.Data.Items.Count);
            }
            else
            {
                var errors = cart.Errors is not null ? string.Join(", ", cart.Errors) : "Unknown error";
                _logger.LogWarning("Failed to get user cart. Errors: {Errors}", errors);
                throw new InvalidOperationException(errors);
            }
        }

        public void ToggleSelectAll(bool value)
        {
            SelectedAll = value;
            RentalCartItems.ForEach(cartItem => cartItem.Selected = value);
            _logger.LogInformation("All rental are selected: {SelectedAll}", SelectedAll);
            OnCartChanged?.Invoke(); // Notify that cart changed to update TotalPrice
        }

        public async Task<ApiResponse<CartItemReadDto>> AddItem(Guid sessionid)
        {
            _logger.LogInformation("Attempting to add (SessionId: {SessionId}) to cart (CartId: {CartId})...",
                sessionid, Id);

            if (Id == Guid.Empty)
                await GetUserCartData();

            var response = await _cartService.AddItemAsync(new CartItemViewModel { CartId = Id, SessionId = sessionid });

            if (response.Success && response.Data != null)
            {
                RentalCartItems.Add(new RentalCartItemViewModel { Item = response.Data });
                _logger.LogInformation(
                    "Successfully added Course '{CourseName}' (CourseId: {CourseId}, SessionId: {SessionId}) to cart (CartId: {CartId}).",
                    response.Data.Course.Name, response.Data.Course.Id, response.Data.SessionId, Id);
            }
            else
            {
                var errors = response.Errors is not null ? string.Join(", ", response.Errors) : "Unknown error";
                _logger.LogError("Failed to add item. Errors: {Errors}", errors);
            }

            OnCartChanged?.Invoke();

            return response;
        }
        public async Task<bool> RemoveSelectedItemsAsync()
        {
            var selectedItems = RentalCartItems.Where(c => c.Selected).ToList();
            if (selectedItems.Count == 0) return true;

            var selectedIds = selectedItems.Select(c => c.Item.Id).ToList();
            var response = await _cartService.BatchDeleteItemsAsync(selectedIds);

            if (response.Success && response.Data != null)
            {
                foreach (var item in selectedItems)
                {
                    RentalCartItems.Remove(item);
                    _logger.LogInformation("Removed {CourseName} from cart.", item.Item.Course.Name);
                }
                OnCartChanged?.Invoke();
                return true;
            }
            else
            {
                var errors = response.Errors is not null ? string.Join(", ", response.Errors) : "Unknown error";
                _logger.LogWarning("Failed to remove selected items. Errors: {Errors}", errors);
                throw new InvalidOperationException(errors);
            }
        }

        public async Task ClearAllAsync()
        {
            if (RentalCartItems.Count == 0) return;

            var allIds = RentalCartItems.Select(c => c.Item.Id).ToList();

            var response = await _cartService.BatchDeleteItemsAsync(allIds);

            if (response.Success && response.Data != null)
            {
                RentalCartItems.Clear();
                _logger.LogInformation("All items removed from cart successfully.");
            }
            else
            {
                var errors = response.Errors is not null ? string.Join(", ", response.Errors) : "Unknown error";
                _logger.LogError("Failed to clear all items. Errors: {Errors}", errors);
                throw new InvalidOperationException(errors);
            }

            OnCartChanged?.Invoke();
        }

        public async Task<bool> RemoveItemAsync(Guid itemId)
        {
            var cartItem = RentalCartItems.FirstOrDefault(item => item.Item.SessionId == itemId);
            if (cartItem == null)
            {
                _logger.LogWarning("Attempted to remove non-existent cart item with ID {ItemId}", itemId);
                return false;
            }

            var courseName = cartItem.Item.Course.Name;
            var response = await _cartService.BatchDeleteItemsAsync([cartItem.Item.Id]);

            if (response.Success && response.Data != null)
            {
                RentalCartItems.Remove(cartItem);
                _logger.LogInformation("Successfully removed course '{CourseName}' from cart", courseName);

                OnCartChanged?.Invoke();
                return true;
            }

            var errors = response.Errors != null ? string.Join(", ", response.Errors) : "Unknown error";
            _logger.LogError("Failed to remove course '{CourseName}' from cart. Errors: {Errors}", courseName, errors);
            return false;
        }

        public async Task FinalizeInstantPaymentAsync(Guid sessionId, Guid transactionId)
        {
            _logger.LogInformation("Starting instant payment for SessionId: {SessionId}, TransactionId: {TransactionId}", sessionId, transactionId);
            SetLoading(true);

            try
            {
                var result = await _transactionService.InstantPaymentAsync(transactionId, sessionId);

                if (result.Success)
                {

                    _logger.LogInformation("Instant payment completed successfully for SessionId: {SessionId}, TransactionId: {TransactionId}", sessionId, transactionId);
                }
                else
                {
                    _logger.LogWarning("Instant payment failed for SessionId: {SessionId}, TransactionId: {TransactionId}. Errors: {Errors}",
                        sessionId, transactionId, result.Errors is not null ? string.Join(", ", result.Errors) : "None");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred during instant payment for SessionId: {SessionId}, TransactionId: {TransactionId}", sessionId, transactionId);
                throw;
            }
            finally
            {
                SetLoading(false);
                _logger.LogInformation("Finished instant payment process for SessionId: {SessionId}, TransactionId: {TransactionId}", sessionId, transactionId);
            }
        }

        public async Task FinalizePaymentAsync(Guid PaymentId)
        {
            if (Id == Guid.Empty)
                await GetUserCartData();


            if (IsCartEmpty)
            {
                _logger.LogWarning("Attempted to finalize payment, but the cart is empty.");
                throw new InvalidOperationException("Cannot finalize order: the cart is empty.");
            }

            var selectedItems = RentalCartItems.Where(x => x.Selected).ToList();

            if (selectedItems.Count == 0)
            {
                _logger.LogWarning("Attempted to finalize payment, but no cars are selected.");
                throw new InvalidOperationException("Cannot finalize order: no cars selected.");
            }


            _logger.LogInformation("Starting payment for {ItemCount} items. Total: {TotalPrice}", selectedItems.Count, TotalPrice);
            SetLoading(true);

            try
            {
                await _transactionService.CheckoutCartAsync(Id, PaymentId, [.. selectedItems.Select(c => c.Item.Id)]);

                _logger.LogInformation("Payment completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment failed for {ItemCount} items. Total: {TotalPrice}", selectedItems.Count, TotalPrice);
                throw;
            }
            finally
            {
                SetLoading(false);
            }
        }

        private void SetLoading(bool value)
        {
            IsLoading = value;
            OnLoadingChanged?.Invoke();
        }

        private int CalculateTotalPrice()
        {
            return RentalCartItems
                .Where(c => c.Selected)
                .Sum(c => c.Item.Course.Price);
        }
    }
}