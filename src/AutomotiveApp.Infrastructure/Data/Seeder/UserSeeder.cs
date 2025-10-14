using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class UserSeeder
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager)
        {
            // Buat roles
            foreach (var roleName in Enum.GetNames(typeof(UserRole)))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
            }

            // Buat user Admin
            if (await userManager.FindByEmailAsync("admin@example.com") == null)
            {
                var admin = new User
                {
                    Id = Guid.Parse("be765a5a-4be8-4704-9514-fb74ebce7a0b"),
                    UserName = "admin@example.com",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "password");
                await userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
            }

            if (await userManager.FindByEmailAsync("buyer@example.com") == null)
            {
                var buyer = new User
                {
                    Id = Guid.Parse("aeafb671-9423-4613-8910-abedcfb48485"),
                    UserName = "buyer@example.com",
                    Email = "buyer@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(buyer, "password");
                await userManager.AddToRoleAsync(buyer, UserRole.Buyer.ToString());
            }
        }
    }
}