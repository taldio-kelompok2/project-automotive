using System.Transactions;
using AutomotiveApp.BlazorUI.Models;
using AutomotiveApp.BlazorUI.Models.Cart;
using AutomotiveApp.BlazorUI.Models.Transaction;
using AutomotiveApp.BlazorUI.Services.Interface;

namespace AutomotiveApp.BlazorUI.Services.Implementation
{
    public class RentalCartService : IRentalCartService
    {
        private readonly ILogger<RentalCartService> _logger;

        public RentalCartService(ILogger<RentalCartService> logger)
        {
            _logger = logger;
        }

        public List<RentalCartViewModel> RentalCart { get; set; } = [];
        public bool IsLoading { get; set; } = false;
        public bool IsCartEmpty => RentalCart.Count == 0;
        public bool SelectedAll { get; set; } = false;
        public int TotalPrice { get; set; } = 0;
        public event Action? OnCartChanged;
        public event Action? OnLoadingChanged;
        public void ToggleSelectAll(bool value)
        {
            SelectedAll = value;
            RentalCart.ForEach(cartItem => cartItem.Selected = value);
            TotalPrice = RentalCart.Where(c => c.Selected).Sum(c => c.Car.Price);
            _logger.LogInformation("All rental are selected: {SelectedAll}", SelectedAll);
        }
        public void AddItem(CarViewModel car, DateTime? rentalDate)
        {
            if (rentalDate == null)
            {
                _logger.LogWarning("Cannot add {CarName} to cart: rental date is null.", car.Name);
                throw new ArgumentNullException(nameof(rentalDate), "Rental date must be selected.");
            }

            if (RentalCart.Any(c => c.Car.Id == car.Id && c.Car.Schedule == rentalDate))
            {
                _logger.LogWarning("Cannot add the same rental on the same date.");
                throw new InvalidOperationException("Cannot add the same rental on the same date.");
            }

            var carCopy = car.Clone(rentalDate.Value);
            var cartItem = new RentalCartViewModel { Car = carCopy };

            RentalCart.Add(cartItem);
            _logger.LogInformation("added {CarName} to cart.", cartItem.Car.Name);

            SelectedAll = false;
            OnCartChanged?.Invoke();
        }
        public bool TryRemoveItem(RentalCartViewModel cartItem)
        {
            if (cartItem != null)
            {
                if (cartItem.Selected)
                    TotalPrice -= cartItem.Car.Price;
                RentalCart.Remove(cartItem);
                _logger.LogInformation("removed {CarName} to cart.", cartItem.Car.Name);
                return true;
            }

            _logger.LogInformation("rental not found in the cart.");
            return false;
        }

        public void ClearAll()
        {
            RentalCart.Clear();
            TotalPrice = 0;

            _logger.LogInformation("removed all items in cart.");
        }

        public async Task FinalizeInstantPaymentAsync(CarViewModel rental, TransactionViewModel transaction, DateTime? rentalDate)
        {
            ArgumentNullException.ThrowIfNull(rental);

            if (rentalDate == null)
            {
                _logger.LogWarning("Cannot add {CarName} to cart: rental date is null.", rental.Name);
                throw new ArgumentNullException(nameof(rentalDate), "Rental date must be selected.");
            }

            _logger.LogInformation("Starting instant payment {PaymentType} for {RentalName}. Price: {RentalPrice}", transaction.Category.ToString(), rental.Name, rental.Price);

            var carCopy = rental.Clone(rentalDate.Value);
            SetLoading(true);

            try
            {
                //TODO: Backend Transaction Request (temporary implementation)
                await Task.Delay(2000);
                _logger.LogInformation("Instant Payment {PaymentType} completed successfully.", transaction.Category.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Payment {PaymentType} failed for {RentalName}. Total: {RentalPrice}", transaction.Category.ToString(), carCopy.Name, carCopy.Price);
                throw;
            }
            finally
            {
                SetLoading(false);
            }

        }

        public async Task FinalizePaymentAsync()
        {
            if (IsCartEmpty)
            {
                _logger.LogWarning("Attempted to finalize payment, but the cart is empty.");
                throw new InvalidOperationException("Cannot finalize order: the cart is empty.");
            }

            var selectedCars = RentalCart.Where(x => x.Selected).ToList();

            if (selectedCars.Count == 0)
            {
                _logger.LogWarning("Attempted to finalize payment, but no cars are selected.");
                throw new InvalidOperationException("Cannot finalize order: no cars selected.");
            }


            _logger.LogInformation("Starting payment for {ItemCount} items. Total: {TotalPrice}", selectedCars.Count, TotalPrice);
            SetLoading(true);

            try
            {
                await Task.Delay(2000);
                foreach (var car in selectedCars)
                {
                    _ = TryRemoveItem(car);
                }

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