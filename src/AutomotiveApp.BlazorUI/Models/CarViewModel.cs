using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models
{
    public class CarViewModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public readonly string ImagePath = "Images/Cars/";
        public string Category { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public int Price { get; set; } = 0;
        public DateTime? Schedule { get; set; } = null;

        public CarViewModel(string category, string name, int price, string imagePath)
        {
            Category = category;
            Name = name;
            Price = price;
            Image = ImagePath + imagePath;
        }

        // temporary
        public CarViewModel() { }

        public CarViewModel Clone()
        {
            return new CarViewModel
            {
                Id = Id,
                Category = Category,
                Name = Name,
                Price = Price,
                Image = Image,
            };
        }

        public CarViewModel Clone(DateTime rentalDate)
        {
            return new CarViewModel
            {
                Id = Id,
                Category = Category,
                Name = Name,
                Price = Price,
                Image = Image,
                Schedule = rentalDate
            };
        }

        public static List<CarViewModel> InitDummyData()
        {
            return
            [
                // SUV
                new CarViewModel("SUV", "Course SUV Kijang Innova", 800_000, "Course-SUV-Kijang-Innova.svg"),
                new CarViewModel("SUV", "Hyundai Palisade 2021", 800_000, "Hyundai-Palisade-2021.svg"),
                new CarViewModel("SUV", "Course Suzuki XL7", 800_000, "Course-Suzuki-XL7.svg"),
                new CarViewModel("SUV", "Course Mitsubishi Pajero", 800_000, "Course-Mitsubishi-Pajero.svg"),
                new CarViewModel("SUV", "SUV Toyota Fortuner", 800_000, "SUV-Toyota-Fortuner.svg"),
                new CarViewModel("SUV", "Premium Mazda CX5 Course", 800_000, "Premium-Mazda-CX5-Course.svg"),

                // LCGC
                new CarViewModel("LCGC", "Toyota Agya 2022", 500_000, "Toyota-Agya-2022.svg"),
                new CarViewModel("LCGC", "Honda Brio Satya", 520_000, "Honda-Brio-Satya.svg"),
                new CarViewModel("LCGC", "Daihatsu Ayla", 480_000, "Daihatsu-Ayla.svg"),
                new CarViewModel("LCGC", "Suzuki Karimun Wagon R", 470_000, "Suzuki-Karimun-WagonR.svg"),
                new CarViewModel("LCGC", "Datsun GO+", 460_000, "Datsun-GO-Plus.svg"),
                new CarViewModel("LCGC", "Wuling Confero S", 490_000, "Wuling-ConferoS.svg"),

                // Truck
                new CarViewModel("Truck", "Mitsubishi Fuso Canter", 1_000_000, "Mitsubishi-Fuso-Canter.svg"),
                new CarViewModel("Truck", "Hino Dutro 130 HD", 1_100_000, "Hino-Dutro-130HD.svg"),
                new CarViewModel("Truck", "Isuzu Giga FVM", 1_200_000, "Isuzu-Giga-FVM.svg"),
                new CarViewModel("Truck", "Mercedes Benz Actros", 2_000_000, "Mercedes-Benz-Actros.svg"),
                new CarViewModel("Truck", "Scania P Series", 2_200_000, "Scania-P-Series.svg"),
                new CarViewModel("Truck", "Volvo FMX", 2_300_000, "Volvo-FMX.svg"),

                // Sedan
                new CarViewModel("Sedan", "Toyota Camry 2022", 900_000, "Toyota-Camry-2022.svg"),
                new CarViewModel("Sedan", "Honda Civic Turbo", 920_000, "Honda-Civic-Turbo.svg"),
                new CarViewModel("Sedan", "Mazda 6", 950_000, "Mazda-6.svg"),
                new CarViewModel("Sedan", "BMW 3 Series", 1_500_000, "BMW-3-Series.svg"),
                new CarViewModel("Sedan", "Mercedes Benz C Class", 1_600_000, "Mercedes-Benz-C-Class.svg"),
                new CarViewModel("Sedan", "Hyundai Elantra", 870_000, "Hyundai-Elantra.svg"),

                // MPV
                new CarViewModel("MPV", "Toyota Avanza Veloz", 800_000, "Toyota-Avanza-Veloz.svg"),
                new CarViewModel("MPV", "Mitsubishi Xpander", 820_000, "Mitsubishi-Xpander.svg"),
                new CarViewModel("MPV", "Honda Mobilio", 780_000, "Honda-Mobilio.svg"),
                new CarViewModel("MPV", "Suzuki Ertiga", 770_000, "Suzuki-Ertiga.svg"),
                new CarViewModel("MPV", "Nissan Livina", 790_000, "Nissan-Livina.svg"),
                new CarViewModel("MPV", "Kia Carnival", 1_200_000, "Kia-Carnival.svg"),

                // Electric
                new CarViewModel("Electric", "Tesla Model 3", 1_500_000, "Tesla-Model-3.svg"),
                new CarViewModel("Electric", "Hyundai Ioniq 5", 1_400_000, "Hyundai-Ioniq-5.svg"),
                new CarViewModel("Electric", "Wuling Air EV", 1_300_000, "Wuling-Air-EV.svg"),
                new CarViewModel("Electric", "Nissan Leaf", 1_350_000, "Nissan-Leaf.svg"),
                new CarViewModel("Electric", "BYD Dolphin", 1_250_000, "BYD-Dolphin.svg"),
                new CarViewModel("Electric", "BMW i3", 1_450_000, "BMW-i3.svg"),

                // Offroad
                new CarViewModel("Offroad", "Jeep Wrangler Rubicon", 1_600_000, "Jeep-Wrangler-Rubicon.svg"),
                new CarViewModel("Offroad", "Toyota Land Cruiser", 1_700_000, "Toyota-Land-Cruiser.svg"),
                new CarViewModel("Offroad", "Suzuki Jimny 4x4", 1_500_000, "Suzuki-Jimny-4x4.svg"),
                new CarViewModel("Offroad", "Ford Bronco", 1_550_000, "Ford-Bronco.svg"),
                new CarViewModel("Offroad", "Land Rover Defender", 1_800_000, "LandRover-Defender.svg"),
                new CarViewModel("Offroad", "Mitsubishi Pajero Sport Dakar", 1_650_000, "Mitsubishi-Pajero-Sport-Dakar.svg"),

                // Hatchback
                new CarViewModel("Hatchback", "Volkswagen Golf GTI", 1_000_000, "Volkswagen-Golf-GTI.svg"),
                new CarViewModel("Hatchback", "Honda Jazz RS", 950_000, "Honda-Jazz-RS.svg"),
                new CarViewModel("Hatchback", "Mazda 2 Hatchback", 980_000, "Mazda-2-Hatchback.svg"),
                new CarViewModel("Hatchback", "Toyota Yaris GR", 970_000, "Toyota-Yaris-GR.svg"),
                new CarViewModel("Hatchback", "Hyundai i20 N", 990_000, "Hyundai-i20-N.svg"),
                new CarViewModel("Hatchback", "Mini Cooper S", 1_400_000, "Mini-Cooper-S.svg")
            ];

        }
    }
}