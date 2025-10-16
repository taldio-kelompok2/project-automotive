using AutomotiveApp.Shared.Dtos.Courses;

namespace AutomotiveApp.BlazorUI.Models.Cart
{
    public class CartItemViewModel
    {
        public Guid CartId { get; set; }
        public Guid SessionId { get; set; }

        // public static List<RentalCartViewModel> InitDummyData()
        // {
        //     var allCars = CarViewModel.InitDummyData();

        //     return [.. allCars.Select(c => new RentalCartViewModel { Car = c })];
        // }

    }
}