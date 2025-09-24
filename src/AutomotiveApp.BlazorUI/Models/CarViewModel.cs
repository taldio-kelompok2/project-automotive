public class CarViewModel
{
    public readonly string ImagePath = "Images/Cars/";
    public CarCategory Category { get; set; } = CarCategory.SUV;
    public string Name { get; set; } = null!;
    public string Image { get; set; } = null!;
    public int Price { get; set; } = 0;

    public CarViewModel(CarCategory category, string name, int price, string imagePath)
    {
        Category = category;
        Name = name;
        Price = price;
        Image = ImagePath + imagePath;
    }

    public static void InitDummyData(List<CarViewModel> cars)
    {
        cars.Add(new CarViewModel(CarCategory.SUV, "Course SUV Kijang Innova", 700_000,
        "inova.svg"));
        cars.Add(new CarViewModel(CarCategory.LCGC, "Course LCGC Honda Brio", 500_000,
        "brio.svg"));
        cars.Add(new CarViewModel(CarCategory.SUV, "Hyundai Palisade 2021", 800_000,
        "palisade.svg"));
        cars.Add(new CarViewModel(CarCategory.SUV, "Course Mitsubishi Pajero", 800_000,
        "pajero.svg"
        ));
        cars.Add(new CarViewModel(CarCategory.Truck, "Dump Truck for Mining", 1_200_000,
        "truck.svg"));
        cars.Add(new CarViewModel(CarCategory.Sedan, "Sedan Honda Civic", 400_000,
        "civic.svg"));
    }
}