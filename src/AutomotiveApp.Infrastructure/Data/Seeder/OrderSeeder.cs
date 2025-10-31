using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Entities.Invoices;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data.Seeder
{
    public static class OrderSeeder
    {
        public static async Task SeedAsync(AppDbContext db, bool reapply = false)
        {
            if (!reapply && await db.Orders.AnyAsync())
                return;

            var buyers = await db.Users
                .Where(u => u.Email != "admin@example.com")
                .ToListAsync();

            var paymentMethods = await db.PaymentMethods.ToListAsync();
            var allSessions = await db.CourseSessions
                .Include(s => s.Course)
                .ToListAsync();

            if (!buyers.Any() || !paymentMethods.Any() || !allSessions.Any())
                return;

            var random = new Random();
            var orders = new List<Order>();
            var orderItems = new List<OrderItem>();
            var invoices = new List<Invoice>();
            var bookings = new List<CourseBooking>();

            int invoiceCounter = 0;
            foreach (var buyer in buyers)
            {
                var buyerCartSessionIds = await db.CartItems
                    .Where(ci => ci.Cart.UserId == buyer.Id)
                    .Select(ci => ci.SessionId)
                    .ToListAsync();

                var availableSessions = allSessions
                    .Where(s => !buyerCartSessionIds.Contains(s.Id))
                    .ToList();

                if (!availableSessions.Any())
                    continue;

                int orderCount = random.Next(1, 3);

                for (int i = 0; i < orderCount; i++)
                {
                    var order = new Order
                    {
                        Id = Guid.NewGuid(),
                        UserId = buyer.Id,
                        PaymentMethodId = paymentMethods[random.Next(paymentMethods.Count)].Id,
                        Status = OrderStatus.Finished,
                        TotalPrice = 0
                    };
                    orders.Add(order);

                    var selectedSessions = availableSessions
                        .OrderBy(_ => random.Next())
                        .Take(random.Next(1, 4))
                        .ToList();

                    long total = 0;
                    foreach (var s in selectedSessions)
                    {
                        var orderItem = new OrderItem
                        {
                            Id = Guid.NewGuid(),
                            OrderId = order.Id,
                            SessionId = s.Id,
                            Price = s.Course.Price
                        };
                        orderItems.Add(orderItem);
                        total += s.Course.Price;

                        var booking = new CourseBooking
                        {
                            Id = Guid.NewGuid(),
                            UserId = buyer.Id,
                            SessionId = s.Id
                        };
                        bookings.Add(booking);
                    }

                    order.TotalPrice = total;
                    invoiceCounter++;

                    var invoice = new Invoice
                    {
                        Id = Guid.NewGuid(),
                        OrderId = order.Id,
                        InvoiceNumber = invoiceCounter,
                        TotalPrice = total
                    };
                    invoices.Add(invoice);
                }
            }

            await db.Orders.AddRangeAsync(orders);
            await db.OrderItems.AddRangeAsync(orderItems);
            await db.SaveChangesAsync();
            await db.Invoices.AddRangeAsync(invoices);
            await db.CourseBookings.AddRangeAsync(bookings);

            await db.SaveChangesAsync();
        }
    }
}
