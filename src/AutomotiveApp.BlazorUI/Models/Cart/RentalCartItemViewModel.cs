namespace AutomotiveApp.BlazorUI.Models.Cart
{
    public class RentalCartViewModel
    {
        public required CarViewModel Car { get; set; }
        public bool Selected { get; set; } = false;

        public static List<RentalCartViewModel> InitDummyData()
        {
            var allCars = CarViewModel.InitDummyData();

            return [.. allCars.Select(c => new RentalCartViewModel { Car = c })];
        }

    }
}