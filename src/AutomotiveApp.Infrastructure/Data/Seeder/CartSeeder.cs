using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class CartSeeder
    {
        public static async Task SeedAsync(AppDbContext db, bool reapply = false)
        {
            Random random = new();

            if (!reapply && await db.Carts.AnyAsync()) return;

            var buyerRoleId = await db.Roles
                .Where(r => r.Name == UserRole.Buyer.ToString())
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            var buyerIds = await db.UserRoles
                .Where(ur => ur.RoleId == buyerRoleId)
                .Join(db.Users, ur => ur.UserId, u => u.Id, (ur, u) => u.Id)
                .OrderBy(_ => Guid.NewGuid())
                .ToListAsync();

            foreach (var buyer in buyerIds)
            {
                var dummyBuyerCart = new Cart
                {
                    TotalPrice = 0,
                    UserId = buyer
                };

                var sessions = await db.CourseSessions
                    .Include(s => s.Course)
                    .ThenInclude(c => c.Category)
                    .OrderBy(s => s.Date)
                    .Take(random.Next(1, 4))
                    .ToListAsync();

                var cartItems = sessions.Select(s => new CartItem
                {
                    Id = Guid.NewGuid(),
                    CartId = dummyBuyerCart.Id,
                    SessionId = s.Id,
                }).ToList();

                await db.Carts.AddAsync(dummyBuyerCart);
                await db.SaveChangesAsync();

                await db.CartItems.AddRangeAsync(cartItems);
                await db.SaveChangesAsync();

                dummyBuyerCart.Items = cartItems;
                dummyBuyerCart.TotalPrice = sessions.Sum(s => s.Course.Price);

                db.Carts.Update(dummyBuyerCart);
                await db.SaveChangesAsync();
            }
        }
    }
}