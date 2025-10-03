using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class CourseSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {

            if (await db.Courses.AnyAsync())
            {
                return;
            }

            var categories = await db.CourseCategories.ToListAsync();
            var categoryLookup = categories.ToDictionary(c => c.Name, c => c);

            var courses = new List<Course>
            {
                // SUV
                new Course { Id = Guid.NewGuid(), Name = "Course SUV Kijang Innova", Price = 800_000, Category = categoryLookup[CarCategory.SUV] },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Palisade 2021", Price = 800_000, Category = categoryLookup[CarCategory.SUV] },
                new Course { Id = Guid.NewGuid(), Name = "Course Suzuki XL7", Price = 800_000, Category = categoryLookup[CarCategory.SUV] },
                new Course { Id = Guid.NewGuid(), Name = "Course Mitsubishi Pajero", Price = 800_000, Category = categoryLookup[CarCategory.SUV] },
                new Course { Id = Guid.NewGuid(), Name = "SUV Toyota Fortuner", Price = 800_000, Category = categoryLookup[CarCategory.SUV] },
                new Course { Id = Guid.NewGuid(), Name = "Premium Mazda CX5 Course", Price = 800_000, Category = categoryLookup[CarCategory.SUV] },

                // LCGC
                new Course { Id = Guid.NewGuid(), Name = "Toyota Agya 2022", Price = 500_000, Category = categoryLookup[CarCategory.LCGC] },
                new Course { Id = Guid.NewGuid(), Name = "Honda Brio Satya", Price = 520_000, Category = categoryLookup[CarCategory.LCGC] },
                new Course { Id = Guid.NewGuid(), Name = "Daihatsu Ayla", Price = 480_000, Category = categoryLookup[CarCategory.LCGC] },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Karimun Wagon R", Price = 470_000, Category = categoryLookup[CarCategory.LCGC] },
                new Course { Id = Guid.NewGuid(), Name = "Datsun GO+", Price = 460_000, Category = categoryLookup[CarCategory.LCGC] },
                new Course { Id = Guid.NewGuid(), Name = "Wuling Confero S", Price = 490_000, Category = categoryLookup[CarCategory.LCGC] },

                // Truck
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Fuso Canter", Price = 1_000_000, Category = categoryLookup[CarCategory.Truck] },
                new Course { Id = Guid.NewGuid(), Name = "Hino Dutro 130 HD", Price = 1_100_000, Category = categoryLookup[CarCategory.Truck] },
                new Course { Id = Guid.NewGuid(), Name = "Isuzu Giga FVM", Price = 1_200_000, Category = categoryLookup[CarCategory.Truck] },
                new Course { Id = Guid.NewGuid(), Name = "Mercedes Benz Actros", Price = 2_000_000, Category = categoryLookup[CarCategory.Truck] },
                new Course { Id = Guid.NewGuid(), Name = "Scania P Series", Price = 2_200_000, Category = categoryLookup[CarCategory.Truck] },
                new Course { Id = Guid.NewGuid(), Name = "Volvo FMX", Price = 2_300_000, Category = categoryLookup[CarCategory.Truck] },

                // Sedan
                new Course { Id = Guid.NewGuid(), Name = "Toyota Camry 2022", Price = 900_000, Category = categoryLookup[CarCategory.Sedan] },
                new Course { Id = Guid.NewGuid(), Name = "Honda Civic Turbo", Price = 920_000, Category = categoryLookup[CarCategory.Sedan] },
                new Course { Id = Guid.NewGuid(), Name = "Mazda 6", Price = 950_000, Category = categoryLookup[CarCategory.Sedan] },
                new Course { Id = Guid.NewGuid(), Name = "BMW 3 Series", Price = 1_500_000, Category = categoryLookup[CarCategory.Sedan] },
                new Course { Id = Guid.NewGuid(), Name = "Mercedes Benz C Class", Price = 1_600_000, Category = categoryLookup[CarCategory.Sedan] },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Elantra", Price = 870_000, Category = categoryLookup[CarCategory.Sedan] },

                // MPV
                new Course { Id = Guid.NewGuid(), Name = "Toyota Avanza Veloz", Price = 800_000, Category = categoryLookup[CarCategory.MPV] },
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Xpander", Price = 820_000, Category = categoryLookup[CarCategory.MPV] },
                new Course { Id = Guid.NewGuid(), Name = "Honda Mobilio", Price = 780_000, Category = categoryLookup[CarCategory.MPV] },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Ertiga", Price = 770_000, Category = categoryLookup[CarCategory.MPV] },
                new Course { Id = Guid.NewGuid(), Name = "Nissan Livina", Price = 790_000, Category = categoryLookup[CarCategory.MPV] },
                new Course { Id = Guid.NewGuid(), Name = "Kia Carnival", Price = 1_200_000, Category = categoryLookup[CarCategory.MPV] },

                // Electric
                new Course { Id = Guid.NewGuid(), Name = "Tesla Model 3", Price = 1_500_000, Category = categoryLookup[CarCategory.Electric] },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Ioniq 5", Price = 1_400_000, Category = categoryLookup[CarCategory.Electric] },
                new Course { Id = Guid.NewGuid(), Name = "Wuling Air EV", Price = 1_300_000, Category = categoryLookup[CarCategory.Electric] },
                new Course { Id = Guid.NewGuid(), Name = "Nissan Leaf", Price = 1_350_000, Category = categoryLookup[CarCategory.Electric] },
                new Course { Id = Guid.NewGuid(), Name = "BYD Dolphin", Price = 1_250_000, Category = categoryLookup[CarCategory.Electric] },
                new Course { Id = Guid.NewGuid(), Name = "BMW i3", Price = 1_450_000, Category = categoryLookup[CarCategory.Electric] },

                // Offroad
                new Course { Id = Guid.NewGuid(), Name = "Jeep Wrangler Rubicon", Price = 1_600_000, Category = categoryLookup[CarCategory.Offroad] },
                new Course { Id = Guid.NewGuid(), Name = "Toyota Land Cruiser", Price = 1_700_000, Category = categoryLookup[CarCategory.Offroad] },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Jimny 4x4", Price = 1_500_000, Category = categoryLookup[CarCategory.Offroad] },
                new Course { Id = Guid.NewGuid(), Name = "Ford Bronco", Price = 1_550_000, Category = categoryLookup[CarCategory.Offroad] },
                new Course { Id = Guid.NewGuid(), Name = "Land Rover Defender", Price = 1_800_000, Category = categoryLookup[CarCategory.Offroad] },
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Pajero Sport Dakar", Price = 1_650_000, Category = categoryLookup[CarCategory.Offroad] },

                // Hatchback
                new Course { Id = Guid.NewGuid(), Name = "Volkswagen Golf GTI", Price = 1_000_000, Category = categoryLookup[CarCategory.Hatchback] },
                new Course { Id = Guid.NewGuid(), Name = "Honda Jazz RS", Price = 950_000, Category = categoryLookup[CarCategory.Hatchback] },
                new Course { Id = Guid.NewGuid(), Name = "Mazda 2 Hatchback", Price = 980_000, Category = categoryLookup[CarCategory.Hatchback] },
                new Course { Id = Guid.NewGuid(), Name = "Toyota Yaris GR", Price = 970_000, Category = categoryLookup[CarCategory.Hatchback] },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai i20 N", Price = 990_000, Category = categoryLookup[CarCategory.Hatchback] },
                new Course { Id = Guid.NewGuid(), Name = "Mini Cooper S", Price = 1_400_000, Category = categoryLookup[CarCategory.Hatchback] }
            };

            await db.Courses.AddRangeAsync(courses);
            await db.SaveChangesAsync();
        }
    }
}