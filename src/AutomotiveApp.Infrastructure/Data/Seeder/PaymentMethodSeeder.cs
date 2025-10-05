using AutomotiveApp.Domain.Entities.Payments;
using AutomotiveApp.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class PaymentMethodSeeder
    {
        public static async Task SeedAsync(AppDbContext db)
        {
            if (await db.PaymentMethods.AnyAsync())
            {
                return;
            }

            var categories = new List<PaymentMethod>();

            foreach (TransactionCategory c in Enum.GetValues<TransactionCategory>())
            {
                categories.Add(new PaymentMethod
                {
                    Id = Guid.NewGuid(),
                    Name = c
                });
            }

            await db.PaymentMethods.AddRangeAsync(categories);
            await db.SaveChangesAsync();
        }
    }
}