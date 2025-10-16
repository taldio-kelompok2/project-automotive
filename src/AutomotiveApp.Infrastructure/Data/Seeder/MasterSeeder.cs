using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Domain.Entities.Courses;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class MasterSeeder
    {
        public static async Task SeedAsync(
            AppDbContext db,
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            bool reapply = false
        )
        {
            if (reapply) await db.Database.EnsureDeletedAsync();
            await db.Database.MigrateAsync();

            // Seed your data
            await CourseCategorySeeder.SeedAsync(db, reapply);
            await PaymentMethodSeeder.SeedAsync(db, reapply);
            await CourseSeeder.SeedAsync(db, reapply);
            await CourseSessionSeeder.SeedAsync(db, reapply);
            await UserSeeder.SeedAsync(userManager, roleManager);
            await CartSeeder.SeedAsync(db, reapply);
        }
    }
}