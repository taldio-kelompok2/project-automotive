using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class CourseCategorySeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (await db.CourseCategories.AnyAsync())
            {
                return;
            }

            var categories = new List<CourseCategory>
            {
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Electric,
                    Description = "Mobil listrik menggunakan baterai dan motor listrik untuk menggerakkan kendaraan, efisiensi tinggi, biaya operasional rendah, ramah lingkungan, serta memiliki teknologi regeneratif pada pengereman."
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Hatchback,
                    Description = "Mobil hatchback memiliki desain kompak, efisien bahan bakar, mudah bermanuver di perkotaan, dengan bagasi terbatas namun fleksibel untuk kebutuhan sehari-hari."
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.LCGC,
                    Description = "LCGC (Low Cost Green Car) merupakan mobil hemat biaya dan ramah lingkungan, dengan fitur dasar, konsumsi bahan bakar efisien, dan biaya perawatan rendah, cocok untuk penggunaan ekonomis."
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.MPV,
                    Description = "MPV memiliki kapasitas penumpang besar, distribusi berat seimbang, nyaman untuk perjalanan keluarga, dengan fokus pada ruang kabin luas dan kenyamanan penumpang."
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Offroad,
                    Description = "Mobil offroad dirancang untuk medan ekstrem, menggunakan sistem penggerak 4WD, suspensi tangguh, kemampuan melewati rintangan berat, dan stabilitas tinggi di berbagai kondisi jalan."
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Sedan,
                    Description = "Sedan memiliki aerodinamika baik, kenyamanan tinggi, performa stabil di jalan raya, desain stylish, dan efisiensi bahan bakar menengah, cocok untuk perjalanan sehari-hari."
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.SUV,
                    Description = "SUV menawarkan kombinasi kenyamanan dan ketangguhan, kapasitas angkut lebih besar, mampu melewati medan ringan hingga sedang, serta fleksibel untuk perjalanan jarak jauh dan perkotaan."
                },
                new CourseCategory
                {
                    Id = Guid.NewGuid(),
                    Name = CarCategory.Truck,
                    Description = "Truk memiliki kapasitas angkut berat, manuver kendaraan besar, fokus pada transportasi barang, efisiensi penggunaan bahan bakar, serta keselamatan dan stabilitas di jalan."
                }
            };

            await db.CourseCategories.AddRangeAsync(categories);
            await db.SaveChangesAsync();
        }
    }
}