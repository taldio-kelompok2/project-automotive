using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class CourseSeeder
    {
        public static async Task SeedAsync(AppDbContext db, bool reapply = false)
        {

            if (!reapply && await db.CourseCategories.AnyAsync())
            {
                return;
            }

            var categories = await db.CourseCategories.ToListAsync();
            var categoryLookup = categories.ToDictionary(c => c.Name, c => c);

            var courses = new List<Course>
            {
                // SUV
                new Course { Id = Guid.NewGuid(), Name = "Course SUV Kijang Innova", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Course-SUV-Kijang-Innova.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Palisade 2021", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Hyundai-Palisade-2021.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Course Suzuki XL7", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Course-Suzuki-XL7.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Course Mitsubishi Pajero", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Course-Mitsubishi-Pajero.svg" },
                new Course { Id = Guid.NewGuid(), Name = "SUV Toyota Fortuner", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "SUV-Toyota-Fortuner.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Premium Mazda CX5 Course", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Premium-Mazda-CX5-Course.svg" },

                // LCGC
                new Course { Id = Guid.NewGuid(), Name = "Toyota Agya 2022", Price = 500_000, Category = categoryLookup["LCGC"], ImageFileName = "Toyota-Agya-2022.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Honda Brio Satya", Price = 520_000, Category = categoryLookup["LCGC"], ImageFileName = "Honda-Brio-Satya.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Daihatsu Ayla", Price = 480_000, Category = categoryLookup["LCGC"], ImageFileName = "Daihatsu-Ayla.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Karimun Wagon R", Price = 470_000, Category = categoryLookup["LCGC"], ImageFileName = "Suzuki-Karimun-WagonR.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Datsun GO+", Price = 460_000, Category = categoryLookup["LCGC"], ImageFileName = "Datsun-GO-plus.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Wuling Confero S", Price = 490_000, Category = categoryLookup["LCGC"], ImageFileName = "Wuling-ConferoS.svg" },

                // Truck
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Fuso Canter", Price = 1_000_000, Category = categoryLookup["Truck"], ImageFileName = "Mitsubishi-Fuso-Canter.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Hino Dutro 130 HD", Price = 1_100_000, Category = categoryLookup["Truck"], ImageFileName = "Hino-Dutro-130HD.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Isuzu Giga FVM", Price = 1_200_000, Category = categoryLookup["Truck"], ImageFileName = "Isuzu-Giga-FVM.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Mercedes Benz Actros", Price = 2_000_000, Category = categoryLookup["Truck"], ImageFileName = "Mercedes-Benz-Actros.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Scania P Series", Price = 2_200_000, Category = categoryLookup["Truck"], ImageFileName = "Scania-P-Series.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Volvo FMX", Price = 2_300_000, Category = categoryLookup["Truck"], ImageFileName = "Volvo-FMX.svg" },

                // Sedan
                new Course { Id = Guid.NewGuid(), Name = "Toyota Camry 2022", Price = 900_000, Category = categoryLookup["Sedan"], ImageFileName = "Toyota-Camry-2022.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Honda Civic Turbo", Price = 920_000, Category = categoryLookup["Sedan"], ImageFileName = "Honda-Civic-Turbo.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Mazda 6", Price = 950_000, Category = categoryLookup["Sedan"], ImageFileName = "Mazda-6.svg" },
                new Course { Id = Guid.NewGuid(), Name = "BMW 3 Series", Price = 1_500_000, Category = categoryLookup["Sedan"], ImageFileName = "BMW-3-Series.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Mercedes Benz C Class", Price = 1_600_000, Category = categoryLookup["Sedan"], ImageFileName = "Mercedes-Benz-C-Class.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Elantra", Price = 870_000, Category = categoryLookup["Sedan"], ImageFileName = "Hyundai-Elantra.svg" },

                // MPV
                new Course { Id = Guid.NewGuid(), Name = "Toyota Avanza Veloz", Price = 800_000, Category = categoryLookup["MPV"], ImageFileName = "Toyota-Avanza-Veloz.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Xpander", Price = 820_000, Category = categoryLookup["MPV"], ImageFileName = "Mitsubishi-Xpander.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Honda Mobilio", Price = 780_000, Category = categoryLookup["MPV"], ImageFileName = "Honda-Mobilio.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Ertiga", Price = 770_000, Category = categoryLookup["MPV"], ImageFileName = "Suzuki-Ertiga.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Nissan Livina", Price = 790_000, Category = categoryLookup["MPV"], ImageFileName = "Nissan-Livina.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Kia Carnival", Price = 1_200_000, Category = categoryLookup["MPV"], ImageFileName = "Kia-Carnival.svg" },

                // Electric
                new Course { Id = Guid.NewGuid(), Name = "Tesla Model 3", Price = 1_500_000, Category = categoryLookup["Electric"], ImageFileName = "Tesla-Model-3.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Ioniq 5", Price = 1_400_000, Category = categoryLookup["Electric"], ImageFileName = "Hyundai-Ioniq-5.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Wuling Air EV", Price = 1_300_000, Category = categoryLookup["Electric"], ImageFileName = "Wuling-Air-EV.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Nissan Leaf", Price = 1_350_000, Category = categoryLookup["Electric"], ImageFileName = "Nissan-Leaf.svg" },
                new Course { Id = Guid.NewGuid(), Name = "BYD Dolphin", Price = 1_250_000, Category = categoryLookup["Electric"], ImageFileName = "BYD-Dolphin.svg" },
                new Course { Id = Guid.NewGuid(), Name = "BMW i3", Price = 1_450_000, Category = categoryLookup["Electric"], ImageFileName = "BMW-i3.svg" },

                // Offroad
                new Course { Id = Guid.NewGuid(), Name = "Jeep Wrangler Rubicon", Price = 1_600_000, Category = categoryLookup["Offroad"], ImageFileName = "Jeep-Wrangler-Rubicon.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Toyota Land Cruiser", Price = 1_700_000, Category = categoryLookup["Offroad"], ImageFileName = "Toyota-Land-Cruiser.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Jimny 4x4", Price = 1_500_000, Category = categoryLookup["Offroad"], ImageFileName = "Suzuki-Jimny-4x4.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Ford Bronco", Price = 1_550_000, Category = categoryLookup["Offroad"], ImageFileName = "Ford-Bronco.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Land Rover Defender", Price = 1_800_000, Category = categoryLookup["Offroad"], ImageFileName = "LandRover-Defender.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Pajero Sport Dakar", Price = 1_650_000, Category = categoryLookup["Offroad"], ImageFileName = "Mitsubishi-Pajero-Sport-Dakar.svg" },

                // Hatchback
                new Course { Id = Guid.NewGuid(), Name = "Volkswagen Golf GTI", Price = 1_000_000, Category = categoryLookup["Hatchback"], ImageFileName = "Volkswagen-Golf-GTI.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Honda Jazz RS", Price = 950_000, Category = categoryLookup["Hatchback"], ImageFileName = "Honda-Jazz-RS.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Mazda 2 Hatchback", Price = 980_000, Category = categoryLookup["Hatchback"], ImageFileName = "Mazda-2-Hatchback.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Toyota Yaris GR", Price = 970_000, Category = categoryLookup["Hatchback"], ImageFileName = "Toyota-Yaris-GR.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai i20 N", Price = 990_000, Category = categoryLookup["Hatchback"], ImageFileName = "Hyundai-i20-N.svg" },
                new Course { Id = Guid.NewGuid(), Name = "Mini Cooper S", Price = 1_400_000, Category = categoryLookup["Hatchback"], ImageFileName = "Mini-Cooper-S.svg" }
            };

            await db.Courses.AddRangeAsync(courses);
            await db.SaveChangesAsync();
        }
    }
}