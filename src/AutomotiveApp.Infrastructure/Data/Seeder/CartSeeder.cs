using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class CartSeeder
    {
        public static async Task SeedAsync(AppDbContext db, bool reapply = false)
        {
            Guid BuyerId = Guid.Parse("aeafb671-9423-4613-8910-abedcfb48485");

            if (!reapply && await db.Carts.AnyAsync())
            {
                return;
            }

            Cart dummyCart = new Cart
            {
                TotalPrice = 0,
                UserId = BuyerId,
            };

            db.Carts.Add(dummyCart);
            await db.SaveChangesAsync();
        }
    }
}