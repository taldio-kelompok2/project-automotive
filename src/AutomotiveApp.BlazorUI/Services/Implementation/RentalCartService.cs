using AutomotiveApp.BlazorUI.Models.Cart;
using AutomotiveApp.BlazorUI.Services.Interface;
namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class RentalCartService : IRentalCartService
    {
        private readonly ILogger<RentalCartService> _logger;
        private readonly ICartService _cartService;
        private readonly ITransactionService _transactionService;

        public Guid UserId { get; set; } = Guid.Parse("aeafb671-9423-4613-8910-abedcfb48485");
        public Guid Id { get; set; }
        public List<RentalCartItemViewModel> RentalCartItems { get; set; } = [];
        public bool IsLoading { get; set; } = false;
        public bool IsCartEmpty => RentalCartItems.Count == 0;
        public bool SelectedAll { get; set; } = false;
        public int TotalPrice { get; set; } = 0;
        public event Action? OnCartChanged;
        public event Action? OnLoadingChanged;

        public RentalCartService(ILogger<RentalCartService> logger, ICartService cartService, ITransactionService transactionService)
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
                RentalCartItems.Clear();
                RentalCartItems.AddRange(cart.Data.Items.Select(item => new RentalCartItemViewModel { Item = item, Selected = false }));
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
            TotalPrice = RentalCartItems.Where(c => c.Selected).Sum(c => c.Item.Course.Price);
            _logger.LogInformation("All rental are selected: {SelectedAll}", SelectedAll);
        }
        public async Task AddItem(Guid sessionid)
        {
            _logger.LogInformation("Attempting to add (SessionId: {SessionId}) to cart (CartId: {CartId})...",
                sessionid, Id);

            if (Id == Guid.Empty)
                await GetUserCartData();

            var item = await _cartService.AddItemAsync(new CartItemViewModel { CartId = Id, SessionId = sessionid });

            if (item.Success && item.Data != null)
            {
                RentalCartItems.Add(new RentalCartItemViewModel { Item = item.Data });
                _logger.LogInformation(
                    "Successfully added Course '{CourseName}' (CourseId: {CourseId}, SessionId: {SessionId}) to cart (CartId: {CartId}).",
                    item.Data.Course.Name, item.Data.Course.Id, item.Data.SessionId, Id);
            }
            else
            {
                var errors = item.Errors is not null ? string.Join(", ", item.Errors) : "Unknown error";
                _logger.LogError("Failed to add item. Errors: {Errors}", errors);
                throw new InvalidOperationException(errors);
            }

            SelectedAll = false;
            OnCartChanged?.Invoke();
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
                    TotalPrice -= item.Item.Course.Price;
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
                TotalPrice = 0;
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

        public async Task<bool> RemoveItemAsync(RentalCartItemViewModel cartItem)
        {
            if (cartItem is null) throw new ArgumentNullException(nameof(cartItem));

            var response = await _cartService.BatchDeleteItemsAsync([cartItem.Item.Id]);

            if (response.Success && response.Data != null)
            {
                if (cartItem.Selected) TotalPrice -= cartItem.Item.Course.Price;
                RentalCartItems.Remove(cartItem);
                _logger.LogInformation("Removed single item {CourseName} from cart.", cartItem.Item.Course.Name);

                OnCartChanged?.Invoke();
                return true;
            }

            var errors = response.Errors is not null ? string.Join(", ", response.Errors) : "Unknown error";
            _logger.LogError("Failed to remove item {CourseName}. Errors: {Errors}", cartItem.Item.Course.Name, errors);
            throw new InvalidOperationException(errors);
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

            var selectedCars = RentalCartItems.Where(x => x.Selected).ToList();

            if (selectedCars.Count == 0)
            {
                _logger.LogWarning("Attempted to finalize payment, but no cars are selected.");
                throw new InvalidOperationException("Cannot finalize order: no cars selected.");
            }


            _logger.LogInformation("Starting payment for {ItemCount} items. Total: {TotalPrice}", selectedCars.Count, TotalPrice);
            SetLoading(true);

            try
            {
                await _transactionService.CheckoutCartAsync(Id, PaymentId, [.. RentalCartItems.Select(c => c.Item.Id)]);
                // await RemoveSelectedItemsAsync();

                _logger.LogInformation("Payment completed successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment failed for {ItemCount} items. Total: {TotalPrice}", selectedCars.Count, TotalPrice);
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
    }
}