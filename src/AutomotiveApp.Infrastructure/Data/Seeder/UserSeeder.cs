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
                var admin = new User { UserName = "admin@example.com", Email = "admin@example.com", EmailConfirmed = true };
                await userManager.CreateAsync(admin, "password");
                await userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
            }

            if (await userManager.FindByEmailAsync("buyer@example.com") == null)
            {
                var buyer = new User
                {
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