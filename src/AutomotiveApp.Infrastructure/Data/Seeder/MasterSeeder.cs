using AutomotiveApp.Domain.Entities.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class MasterSeeder
    {
        public static async Task SeedAsync(
            AppDbContext db,
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager
        )
        {

            await db.Database.MigrateAsync();
            await CourseCategorySeeder.SeedAsync(db);
            await PaymentMethodSeeder.SeedAsync(db);
            await CourseSeeder.SeedAsync(db);
            await UserSeeder.SeedAsync(userManager, roleManager);
        }
    }
}