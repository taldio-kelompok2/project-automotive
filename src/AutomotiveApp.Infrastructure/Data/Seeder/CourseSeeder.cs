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
                new Course { Id = Guid.NewGuid(), Name = "Course SUV Kijang Innova", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Course-SUV-Kijang-Innova.svg", Description = "A versatile SUV perfect for family trips, offering ample space, comfort, and reliability for daily commutes or weekend adventures." },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Palisade 2021", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Hyundai-Palisade-2021.svg", Description = "Luxury SUV with modern technology, premium interiors, and smooth handling, designed to provide a first-class driving experience." },
                new Course { Id = Guid.NewGuid(), Name = "Course Suzuki XL7", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Course-Suzuki-XL7.svg", Description = "Compact yet spacious SUV combining fuel efficiency with modern features, ideal for city driving and family use." },
                new Course { Id = Guid.NewGuid(), Name = "Course Mitsubishi Pajero", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Course-Mitsubishi-Pajero.svg", Description = "A rugged SUV built for off-road adventures, offering powerful performance, durability, and exceptional stability on challenging terrains." },
                new Course { Id = Guid.NewGuid(), Name = "SUV Toyota Fortuner", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "SUV-Toyota-Fortuner.svg", Description = "Powerful SUV with advanced safety features, comfortable seating, and reliable performance, perfect for both city streets and rough terrains." },
                new Course { Id = Guid.NewGuid(), Name = "Premium Mazda CX5 Course", Price = 800_000, Category = categoryLookup["SUV"], ImageFileName = "Premium-Mazda-CX5-Course.svg", Description = "Premium SUV offering sleek design, high-end features, and a smooth driving experience for those who value comfort and style." },

                // LCGC
                new Course { Id = Guid.NewGuid(), Name = "Toyota Agya 2022", Price = 500_000, Category = categoryLookup["LCGC"], ImageFileName = "Toyota-Agya-2022.svg", Description = "Compact and affordable city car, perfect for daily commuting with low fuel consumption and easy maneuverability." },
                new Course { Id = Guid.NewGuid(), Name = "Honda Brio Satya", Price = 520_000, Category = categoryLookup["LCGC"], ImageFileName = "Honda-Brio-Satya.svg", Description = "Small yet stylish hatchback combining efficiency and practicality, ideal for navigating crowded urban streets." },
                new Course { Id = Guid.NewGuid(), Name = "Daihatsu Ayla", Price = 480_000, Category = categoryLookup["LCGC"], ImageFileName = "Daihatsu-Ayla.svg", Description = "Budget-friendly car offering reliable performance and low maintenance costs, making it perfect for first-time car owners." },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Karimun Wagon R", Price = 470_000, Category = categoryLookup["LCGC"], ImageFileName = "Suzuki-Karimun-WagonR.svg", Description = "Compact hatchback designed for city living, featuring practical storage and excellent fuel economy." },
                new Course { Id = Guid.NewGuid(), Name = "Datsun GO+", Price = 460_000, Category = categoryLookup["LCGC"], ImageFileName = "Datsun-GO-plus.svg", Description = "Economical multi-purpose vehicle suitable for small families, offering a balance of comfort and affordability." },
                new Course { Id = Guid.NewGuid(), Name = "Wuling Confero S", Price = 490_000, Category = categoryLookup["LCGC"], ImageFileName = "Wuling-ConferoS.svg", Description = "Practical and spacious vehicle ideal for city and occasional long trips, combining functionality with affordability." },

                // Truck
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Fuso Canter", Price = 1_000_000, Category = categoryLookup["Truck"], ImageFileName = "Mitsubishi-Fuso-Canter.svg", Description = "Reliable light truck perfect for urban deliveries, offering efficiency, durability, and ease of handling." },
                new Course { Id = Guid.NewGuid(), Name = "Hino Dutro 130 HD", Price = 1_100_000, Category = categoryLookup["Truck"], ImageFileName = "Hino-Dutro-130HD.svg", Description = "Medium-duty truck with strong load capacity, optimized for commercial transport and long-lasting performance." },
                new Course { Id = Guid.NewGuid(), Name = "Isuzu Giga FVM", Price = 1_200_000, Category = categoryLookup["Truck"], ImageFileName = "Isuzu-Giga-FVM.svg", Description = "Heavy-duty truck designed for large cargo and long-distance hauling, built to withstand tough conditions." },
                new Course { Id = Guid.NewGuid(), Name = "Mercedes Benz Actros", Price = 2_000_000, Category = categoryLookup["Truck"], ImageFileName = "Mercedes-Benz-Actros.svg", Description = "Premium truck delivering unmatched comfort, safety, and reliability for professional long-haul drivers." },
                new Course { Id = Guid.NewGuid(), Name = "Scania P Series", Price = 2_200_000, Category = categoryLookup["Truck"], ImageFileName = "Scania-P-Series.svg", Description = "High-performance truck combining efficiency, power, and durability for demanding transport needs." },
                new Course { Id = Guid.NewGuid(), Name = "Volvo FMX", Price = 2_300_000, Category = categoryLookup["Truck"], ImageFileName = "Volvo-FMX.svg", Description = "Rugged truck designed for construction and off-road transport, offering safety, comfort, and excellent load handling." },

                // Sedan
                new Course { Id = Guid.NewGuid(), Name = "Toyota Camry 2022", Price = 900_000, Category = categoryLookup["Sedan"], ImageFileName = "Toyota-Camry-2022.svg", Description = "Elegant sedan with advanced features, comfortable interiors, and smooth performance for city and highway driving." },
                new Course { Id = Guid.NewGuid(), Name = "Honda Civic Turbo", Price = 920_000, Category = categoryLookup["Sedan"], ImageFileName = "Honda-Civic-Turbo.svg", Description = "Sporty and stylish sedan offering responsive handling, turbocharged performance, and modern tech features." },
                new Course { Id = Guid.NewGuid(), Name = "Mazda 6", Price = 950_000, Category = categoryLookup["Sedan"], ImageFileName = "Mazda-6.svg", Description = "Premium sedan combining sleek design, comfort, and efficient performance for daily commutes or long drives." },
                new Course { Id = Guid.NewGuid(), Name = "BMW 3 Series", Price = 1_500_000, Category = categoryLookup["Sedan"], ImageFileName = "BMW-3-Series.svg", Description = "Luxury sports sedan with high performance, advanced features, and exceptional driving dynamics." },
                new Course { Id = Guid.NewGuid(), Name = "Mercedes Benz C Class", Price = 1_600_000, Category = categoryLookup["Sedan"], ImageFileName = "Mercedes-Benz-C-Class.svg", Description = "Sophisticated sedan with premium interiors, innovative technology, and superior comfort for long journeys." },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Elantra", Price = 870_000, Category = categoryLookup["Sedan"], ImageFileName = "Hyundai-Elantra.svg", Description = "Reliable and stylish sedan offering excellent fuel economy, comfort, and modern infotainment options." },

                // MPV
                new Course { Id = Guid.NewGuid(), Name = "Toyota Avanza Veloz", Price = 800_000, Category = categoryLookup["MPV"], ImageFileName = "Toyota-Avanza-Veloz.svg", Description = "Family-friendly MPV with spacious interiors, smooth handling, and versatile seating arrangements for everyone." },
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Xpander", Price = 820_000, Category = categoryLookup["MPV"], ImageFileName = "Mitsubishi-Xpander.svg", Description = "Stylish MPV combining comfort, practicality, and modern features for family and group travels." },
                new Course { Id = Guid.NewGuid(), Name = "Honda Mobilio", Price = 780_000, Category = categoryLookup["MPV"], ImageFileName = "Honda-Mobilio.svg", Description = "Compact MPV offering fuel efficiency, flexible seating, and practicality for city and weekend trips." },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Ertiga", Price = 770_000, Category = categoryLookup["MPV"], ImageFileName = "Suzuki-Ertiga.svg", Description = "Reliable MPV perfect for small families, combining comfort, modern features, and a smooth ride." },
                new Course { Id = Guid.NewGuid(), Name = "Nissan Livina", Price = 790_000, Category = categoryLookup["MPV"], ImageFileName = "Nissan-Livina.svg", Description = "Spacious and practical MPV with enhanced comfort, ideal for daily use or family getaways." },
                new Course { Id = Guid.NewGuid(), Name = "Kia Carnival", Price = 1_200_000, Category = categoryLookup["MPV"], ImageFileName = "Kia-Carnival.svg", Description = "Premium MPV designed for ultimate comfort, luxury, and advanced technology, perfect for long-distance travels." },

                // Electric
                new Course { Id = Guid.NewGuid(), Name = "Tesla Model 3", Price = 1_500_000, Category = categoryLookup["Electric"], ImageFileName = "Tesla-Model-3.svg", Description = "High-performance electric vehicle with advanced autopilot, long-range battery, and sleek aerodynamic design." },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai Ioniq 5", Price = 1_400_000, Category = categoryLookup["Electric"], ImageFileName = "Hyundai-Ioniq-5.svg", Description = "Futuristic electric SUV with fast charging, spacious interior, and cutting-edge technology." },
                new Course { Id = Guid.NewGuid(), Name = "Wuling Air EV", Price = 1_300_000, Category = categoryLookup["Electric"], ImageFileName = "Wuling-Air-EV.svg", Description = "Compact electric car offering practicality, eco-friendliness, and low running costs for city driving." },
                new Course { Id = Guid.NewGuid(), Name = "Nissan Leaf", Price = 1_350_000, Category = categoryLookup["Electric"], ImageFileName = "Nissan-Leaf.svg", Description = "Well-known electric hatchback providing efficient commuting, modern tech, and zero emissions driving experience." },
                new Course { Id = Guid.NewGuid(), Name = "BYD Dolphin", Price = 1_250_000, Category = categoryLookup["Electric"], ImageFileName = "BYD-Dolphin.svg", Description = "Small and stylish electric vehicle with easy maneuverability, ideal for eco-conscious city drivers." },
                new Course { Id = Guid.NewGuid(), Name = "BMW i3", Price = 1_450_000, Category = categoryLookup["Electric"], ImageFileName = "BMW-i3.svg", Description = "Luxury electric car combining compact design, innovative technology, and premium comfort for urban journeys." },

                // Offroad
                new Course { Id = Guid.NewGuid(), Name = "Jeep Wrangler Rubicon", Price = 1_600_000, Category = categoryLookup["Offroad"], ImageFileName = "Jeep-Wrangler-Rubicon.svg", Description = "Legendary off-road SUV built for rugged adventures, offering superior traction and durability in extreme conditions." },
                new Course { Id = Guid.NewGuid(), Name = "Toyota Land Cruiser", Price = 1_700_000, Category = categoryLookup["Offroad"], ImageFileName = "Toyota-Land-Cruiser.svg", Description = "Powerful off-road vehicle combining luxury, comfort, and unmatched reliability for challenging terrains." },
                new Course { Id = Guid.NewGuid(), Name = "Suzuki Jimny 4x4", Price = 1_500_000, Category = categoryLookup["Offroad"], ImageFileName = "Suzuki-Jimny-4x4.svg", Description = "Compact 4x4 SUV ideal for off-road trails, combining agility, robustness, and fun driving experience." },
                new Course { Id = Guid.NewGuid(), Name = "Ford Bronco", Price = 1_550_000, Category = categoryLookup["Offroad"], ImageFileName = "Ford-Bronco.svg", Description = "Versatile off-road SUV offering rugged performance, advanced safety features, and comfort for adventure enthusiasts." },
                new Course { Id = Guid.NewGuid(), Name = "Land Rover Defender", Price = 1_800_000, Category = categoryLookup["Offroad"], ImageFileName = "LandRover-Defender.svg", Description = "Iconic off-road SUV built to tackle the toughest terrains, combining heritage, luxury, and advanced technology." },
                new Course { Id = Guid.NewGuid(), Name = "Mitsubishi Pajero Sport Dakar", Price = 1_650_000, Category = categoryLookup["Offroad"], ImageFileName = "Mitsubishi-Pajero-Sport-Dakar.svg", Description = "Rugged SUV with high performance, ideal for extreme adventures while maintaining comfort and safety." },

                // Hatchback
                new Course { Id = Guid.NewGuid(), Name = "Volkswagen Golf GTI", Price = 1_000_000, Category = categoryLookup["Hatchback"], ImageFileName = "Volkswagen-Golf-GTI.svg", Description = "Sporty hatchback offering dynamic driving, sleek design, and responsive performance for enthusiasts." },
                new Course { Id = Guid.NewGuid(), Name = "Honda Jazz RS", Price = 950_000, Category = categoryLookup["Hatchback"], ImageFileName = "Honda-Jazz-RS.svg", Description = "Stylish and practical hatchback featuring modern technology, comfortable interiors, and fuel efficiency." },
                new Course { Id = Guid.NewGuid(), Name = "Mazda 2 Hatchback", Price = 980_000, Category = categoryLookup["Hatchback"], ImageFileName = "Mazda-2-Hatchback.svg", Description = "Compact hatchback combining sporty design, smooth handling, and advanced safety features for urban driving." },
                new Course { Id = Guid.NewGuid(), Name = "Toyota Yaris GR", Price = 970_000, Category = categoryLookup["Hatchback"], ImageFileName = "Toyota-Yaris-GR.svg", Description = "High-performance hatchback delivering thrilling driving experience, sporty styling, and reliability." },
                new Course { Id = Guid.NewGuid(), Name = "Hyundai i20 N", Price = 990_000, Category = categoryLookup["Hatchback"], ImageFileName = "Hyundai-i20-N.svg", Description = "Fun-to-drive hatchback with advanced performance features, designed for spirited urban driving." },
                new Course { Id = Guid.NewGuid(), Name = "Mini Cooper S", Price = 1_400_000, Category = categoryLookup["Hatchback"], ImageFileName = "Mini-Cooper-S.svg", Description = "Iconic and sporty hatchback with premium interiors, unique design, and enjoyable driving dynamics." }
            };


            await db.Courses.AddRangeAsync(courses);
            await db.SaveChangesAsync();
        }
    }
}