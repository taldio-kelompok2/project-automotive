using AutomotiveApp.Domain.Entities.Payments;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class PaymentMethodSeeder
    {
        public static async Task SeedAsync(AppDbContext db, bool reapply = false)
        {
            if (!reapply && await db.CourseCategories.AnyAsync())
            {
                return;
            }

            var categories = new List<PaymentMethod>();

            foreach (TransactionCategory c in Enum.GetValues<TransactionCategory>())
            {
                categories.Add(new PaymentMethod
                {
                    Id = Guid.NewGuid(),
                    Name = c.ToString()
                });
            }

            await db.PaymentMethods.AddRangeAsync(categories);
            await db.SaveChangesAsync();
        }
    }
}