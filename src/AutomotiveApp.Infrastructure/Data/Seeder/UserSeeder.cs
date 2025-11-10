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
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(admin, "password");
                await userManager.AddToRoleAsync(admin, UserRole.Admin.ToString());
            }

            // Buat user buyer

            if (await userManager.FindByEmailAsync("buyer@example.com") == null)
            {
                var buyer = new User
                {
                    Id = Guid.Parse("aeafb671-9423-4613-8910-abedcfb48485"),
                    UserName = "buyer",
                    Email = "buyer@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(buyer, "password");
                await userManager.AddToRoleAsync(buyer, UserRole.Buyer.ToString());
            }

            if (await userManager.FindByEmailAsync("nathan.sutanto@example.com") == null)
            {
                var buyer1 = new User
                {
                    Id = Guid.Parse("b9a24c8e-3b6a-4d61-bb51-8c2b6f4a1a19"),
                    UserName = "nathan.sutanto",
                    Email = "nathan.sutanto@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(buyer1, "password");
                await userManager.AddToRoleAsync(buyer1, UserRole.Buyer.ToString());
            }

            if (await userManager.FindByEmailAsync("melissa.tan@example.com") == null)
            {
                var buyer2 = new User
                {
                    Id = Guid.Parse("3b4e5c3f-5d39-4bdf-9cc9-6f6b8d18ce20"),
                    UserName = "melissa.tan",
                    Email = "melissa.tan@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(buyer2, "password");
                await userManager.AddToRoleAsync(buyer2, UserRole.Buyer.ToString());
            }

            if (await userManager.FindByEmailAsync("jason.lim@example.com") == null)
            {
                var buyer3 = new User
                {
                    Id = Guid.Parse("7d65b1a2-6f49-4a8b-bfd8-b9f4a33d3921"),
                    UserName = "jason.lim",
                    Email = "jason.lim@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(buyer3, "password");
                await userManager.AddToRoleAsync(buyer3, UserRole.Buyer.ToString());
            }

            if (await userManager.FindByEmailAsync("clara.putri@example.com") == null)
            {
                var buyer4 = new User
                {
                    Id = Guid.Parse("c12e3ab4-3e32-4d88-bb4d-df5e62c5a22e"),
                    UserName = "clara.putri",
                    Email = "clara.putri@example.com",
                    EmailConfirmed = true
                };

                await userManager.CreateAsync(buyer4, "password");
                await userManager.AddToRoleAsync(buyer4, UserRole.Buyer.ToString());
            }

        }
    }
}