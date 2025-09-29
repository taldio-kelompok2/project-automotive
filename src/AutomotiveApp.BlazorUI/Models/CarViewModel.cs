using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models
{
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

        // temporary
        public CarViewModel() { }

        public static void InitDummyData(List<CarViewModel> cars)
        {
            //cars.Add(new CarViewModel(CarCategory.SUV, "Course SUV Kijang Innova", 700_000,
            //"inova.svg"));
            //cars.Add(new CarViewModel(CarCategory.LCGC, "Course LCGC Honda Brio", 500_000,
            //"brio.svg"));
            //cars.Add(new CarViewModel(CarCategory.SUV, "Hyundai Palisade 2021", 800_000,
            //"palisade.svg"));
            //cars.Add(new CarViewModel(CarCategory.SUV, "Course Mitsubishi Pajero", 800_000,
            //"pajero.svg"
            //));
            //cars.Add(new CarViewModel(CarCategory.Truck, "Dump Truck for Mining", 1_200_000,
            //"truck.svg"));
            //cars.Add(new CarViewModel(CarCategory.Sedan, "Sedan Honda Civic", 400_000,
            //"civic.svg"));

            // SUV
            cars.Add(new CarViewModel(CarCategory.SUV, "Course SUV Kijang Innova", 800_000, "Course-SUV-Kijang-Innova.svg"));
            cars.Add(new CarViewModel(CarCategory.SUV, "Hyundai Palisade 2021", 800_000, "Hyundai-Palisade-2021.svg"));
            cars.Add(new CarViewModel(CarCategory.SUV, "Course Suzuki XL7", 800_000, "Course-Suzuki-XL7.svg"));
            cars.Add(new CarViewModel(CarCategory.SUV, "Course Mitsubishi Pajero", 800_000, "Course-Mitsubishi-Pajero.svg"));
            cars.Add(new CarViewModel(CarCategory.SUV, "SUV Toyota Fortuner", 800_000, "SUV-Toyota-Fortuner.svg"));
            cars.Add(new CarViewModel(CarCategory.SUV, "Premium Mazda CX5 Course", 800_000, "Premium-Mazda-CX5-Course.svg"));

            // LCGC
            cars.Add(new CarViewModel(CarCategory.LCGC, "Toyota Agya 2022", 500_000, "Toyota-Agya-2022.svg"));
            cars.Add(new CarViewModel(CarCategory.LCGC, "Honda Brio Satya", 520_000, "Honda-Brio-Satya.svg"));
            cars.Add(new CarViewModel(CarCategory.LCGC, "Daihatsu Ayla", 480_000, "Daihatsu-Ayla.svg"));
            cars.Add(new CarViewModel(CarCategory.LCGC, "Suzuki Karimun Wagon R", 470_000, "Suzuki-Karimun-WagonR.svg"));
            cars.Add(new CarViewModel(CarCategory.LCGC, "Datsun GO+", 460_000, "Datsun-GO-Plus.svg"));
            cars.Add(new CarViewModel(CarCategory.LCGC, "Wuling Confero S", 490_000, "Wuling-ConferoS.svg"));

            // Truck
            cars.Add(new CarViewModel(CarCategory.Truck, "Mitsubishi Fuso Canter", 1_000_000, "Mitsubishi-Fuso-Canter.svg"));
            cars.Add(new CarViewModel(CarCategory.Truck, "Hino Dutro 130 HD", 1_100_000, "Hino-Dutro-130HD.svg"));
            cars.Add(new CarViewModel(CarCategory.Truck, "Isuzu Giga FVM", 1_200_000, "Isuzu-Giga-FVM.svg"));
            cars.Add(new CarViewModel(CarCategory.Truck, "Mercedes Benz Actros", 2_000_000, "Mercedes-Benz-Actros.svg"));
            cars.Add(new CarViewModel(CarCategory.Truck, "Scania P Series", 2_200_000, "Scania-P-Series.svg"));
            cars.Add(new CarViewModel(CarCategory.Truck, "Volvo FMX", 2_300_000, "Volvo-FMX.svg"));

            // Sedan
            cars.Add(new CarViewModel(CarCategory.Sedan, "Toyota Camry 2022", 900_000, "Toyota-Camry-2022.svg"));
            cars.Add(new CarViewModel(CarCategory.Sedan, "Honda Civic Turbo", 920_000, "Honda-Civic-Turbo.svg"));
            cars.Add(new CarViewModel(CarCategory.Sedan, "Mazda 6", 950_000, "Mazda-6.svg"));
            cars.Add(new CarViewModel(CarCategory.Sedan, "BMW 3 Series", 1_500_000, "BMW-3-Series.svg"));
            cars.Add(new CarViewModel(CarCategory.Sedan, "Mercedes Benz C Class", 1_600_000, "Mercedes-Benz-C-Class.svg"));
            cars.Add(new CarViewModel(CarCategory.Sedan, "Hyundai Elantra", 870_000, "Hyundai-Elantra.svg"));

            // MPV
            cars.Add(new CarViewModel(CarCategory.MPV, "Toyota Avanza Veloz", 800_000, "Toyota-Avanza-Veloz.svg"));
            cars.Add(new CarViewModel(CarCategory.MPV, "Mitsubishi Xpander", 820_000, "Mitsubishi-Xpander.svg"));
            cars.Add(new CarViewModel(CarCategory.MPV, "Honda Mobilio", 780_000, "Honda-Mobilio.svg"));
            cars.Add(new CarViewModel(CarCategory.MPV, "Suzuki Ertiga", 770_000, "Suzuki-Ertiga.svg"));
            cars.Add(new CarViewModel(CarCategory.MPV, "Nissan Livina", 790_000, "Nissan-Livina.svg"));
            cars.Add(new CarViewModel(CarCategory.MPV, "Kia Carnival", 1_200_000, "Kia-Carnival.svg"));

            // Electric
            cars.Add(new CarViewModel(CarCategory.Electric, "Tesla Model 3", 1_500_000, "Tesla-Model-3.svg"));
            cars.Add(new CarViewModel(CarCategory.Electric, "Hyundai Ioniq 5", 1_400_000, "Hyundai-Ioniq-5.svg"));
            cars.Add(new CarViewModel(CarCategory.Electric, "Wuling Air EV", 1_300_000, "Wuling-Air-EV.svg"));
            cars.Add(new CarViewModel(CarCategory.Electric, "Nissan Leaf", 1_350_000, "Nissan-Leaf.svg"));
            cars.Add(new CarViewModel(CarCategory.Electric, "BYD Dolphin", 1_250_000, "BYD-Dolphin.svg"));
            cars.Add(new CarViewModel(CarCategory.Electric, "BMW i3", 1_450_000, "BMW-i3.svg"));

            // Offroad
            cars.Add(new CarViewModel(CarCategory.Offroad, "Jeep Wrangler Rubicon", 1_600_000, "Jeep-Wrangler-Rubicon.svg"));
            cars.Add(new CarViewModel(CarCategory.Offroad, "Toyota Land Cruiser", 1_700_000, "Toyota-Land-Cruiser.svg"));
            cars.Add(new CarViewModel(CarCategory.Offroad, "Suzuki Jimny 4x4", 1_500_000, "Suzuki-Jimny-4x4.svg"));
            cars.Add(new CarViewModel(CarCategory.Offroad, "Ford Bronco", 1_550_000, "Ford-Bronco.svg"));
            cars.Add(new CarViewModel(CarCategory.Offroad, "Land Rover Defender", 1_800_000, "LandRover-Defender.svg"));
            cars.Add(new CarViewModel(CarCategory.Offroad, "Mitsubishi Pajero Sport Dakar", 1_650_000, "Mitsubishi-Pajero-Sport-Dakar.svg"));

            // Hatchback
            cars.Add(new CarViewModel(CarCategory.Hatchback, "Volkswagen Golf GTI", 1_000_000, "Volkswagen-Golf-GTI.svg"));
            cars.Add(new CarViewModel(CarCategory.Hatchback, "Honda Jazz RS", 950_000, "Honda-Jazz-RS.svg"));
            cars.Add(new CarViewModel(CarCategory.Hatchback, "Mazda 2 Hatchback", 980_000, "Mazda-2-Hatchback.svg"));
            cars.Add(new CarViewModel(CarCategory.Hatchback, "Toyota Yaris GR", 970_000, "Toyota-Yaris-GR.svg"));
            cars.Add(new CarViewModel(CarCategory.Hatchback, "Hyundai i20 N", 990_000, "Hyundai-i20-N.svg"));
            cars.Add(new CarViewModel(CarCategory.Hatchback, "Mini Cooper S", 1_400_000, "Mini-Cooper-S.svg"));

        }
    }
}