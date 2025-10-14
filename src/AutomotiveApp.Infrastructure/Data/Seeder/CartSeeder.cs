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

            var sessions = await db.CourseSessions
                .Include(s => s.Course)
                .ThenInclude(c => c.Category)
                .OrderBy(s => s.Date)
                .Take(3)
                .ToListAsync();

            var cartItems = sessions.Select(s => new CartItem
            {
                Id = Guid.NewGuid(),
                CartId = dummyCart.Id,
                SessionId = s.Id,
            }).ToList();

            await db.Carts.AddAsync(dummyCart);
            await db.SaveChangesAsync();

            await db.CartItems.AddRangeAsync(cartItems);
            await db.SaveChangesAsync();

            dummyCart.Items = cartItems;
            dummyCart.TotalPrice = sessions.Sum(s => s.Course.Price);

            db.Carts.Update(dummyCart);
            await db.SaveChangesAsync();

        }
    }
}