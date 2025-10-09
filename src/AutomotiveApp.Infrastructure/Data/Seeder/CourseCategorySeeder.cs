using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class CourseCategorySeeder
    {
        public static async Task SeedAsync(AppDbContext db, bool reapply = false)
        {
            if (!reapply && await db.CourseCategories.AnyAsync())
            {
                return;
            }

            var categories = new List<CourseCategory>
            {
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Electric.ToString(),
                    Description = "Mobil listrik menggunakan baterai dan motor listrik untuk menggerakkan kendaraan, efisiensi tinggi, biaya operasional rendah, ramah lingkungan, serta memiliki teknologi regeneratif pada pengereman.",
                    ImageFileName = $"{CarCategory.Electric.ToString().ToLower()}.svg"
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Hatchback.ToString(),
                    Description = "Mobil hatchback memiliki desain kompak, efisien bahan bakar, mudah bermanuver di perkotaan, dengan bagasi terbatas namun fleksibel untuk kebutuhan sehari-hari.",
                    ImageFileName = $"{CarCategory.Hatchback.ToString().ToLower()}.svg"
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.LCGC.ToString(),
                    Description = "LCGC (Low Cost Green Car) merupakan mobil hemat biaya dan ramah lingkungan, dengan fitur dasar, konsumsi bahan bakar efisien, dan biaya perawatan rendah, cocok untuk penggunaan ekonomis.",
                    ImageFileName = $"{CarCategory.LCGC.ToString().ToLower()}.svg"
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.MPV.ToString(),
                    Description = "MPV memiliki kapasitas penumpang besar, distribusi berat seimbang, nyaman untuk perjalanan keluarga, dengan fokus pada ruang kabin luas dan kenyamanan penumpang.",
                    ImageFileName = $"{CarCategory.MPV.ToString().ToLower()}.svg"
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Offroad.ToString(),
                    Description = "Mobil offroad dirancang untuk medan ekstrem, menggunakan sistem penggerak 4WD, suspensi tangguh, kemampuan melewati rintangan berat, dan stabilitas tinggi di berbagai kondisi jalan.",
                    ImageFileName = $"{CarCategory.Offroad.ToString().ToLower()}.svg"
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Sedan.ToString(),
                    Description = "Sedan memiliki aerodinamika baik, kenyamanan tinggi, performa stabil di jalan raya, desain stylish, dan efisiensi bahan bakar menengah, cocok untuk perjalanan sehari-hari.",
                    ImageFileName = $"{CarCategory.Sedan.ToString().ToLower()}.svg"
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.SUV.ToString(),
                    Description = "SUV menawarkan kombinasi kenyamanan dan ketangguhan, kapasitas angkut lebih besar, mampu melewati medan ringan hingga sedang, serta fleksibel untuk perjalanan jarak jauh dan perkotaan.",
                    ImageFileName = $"{CarCategory.SUV.ToString().ToLower()}.svg"
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Truck.ToString(),
                    Description = "Truk memiliki kapasitas angkut berat, manuver kendaraan besar, fokus pada transportasi barang, efisiensi penggunaan bahan bakar, serta keselamatan dan stabilitas di jalan.",
                    ImageFileName = $"{CarCategory.Truck.ToString().ToLower()}.svg"
                }
            };

            await db.CourseCategories.AddRangeAsync(categories);
            await db.SaveChangesAsync();
        }
    }
}