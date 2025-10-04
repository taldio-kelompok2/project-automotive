using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Domain.Entities.Invoices;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Domain.Entities.Payments;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutomotiveApp.Infrastructure.Data
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>

    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseCategory> CourseCategories { get; set; }
        public DbSet<CourseSession> CourseSessions { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }

        //DB Configuration
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Relations

            // User - Order
            modelBuilder.Entity<User>()
                .HasMany(u => u.Orders)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId);

            // User - Cart
            modelBuilder.Entity<User>()
                .HasOne(u => u.Cart)
                .WithOne(ca => ca.User)
                .HasForeignKey<Cart>(ca => ca.UserId);

            // User - CourseBooking
            modelBuilder.Entity<User>()
                .HasMany(u => u.Bookings)
                .WithOne(cb => cb.User)
                .HasForeignKey(cb => cb.UserId);

            // Course - CourseCategory
            modelBuilder.Entity<Course>()
                .HasOne(co => co.Category)
                .WithMany(ca => ca.Courses)
                .HasForeignKey(co => co.CategoryId);

            // Course - CourseSession
            modelBuilder.Entity<Course>()
                .HasMany(co => co.Sessions)
                .WithOne(cs => cs.Course)
                .HasForeignKey(cs => cs.CourseId);

            // CourseSession - OrderItem
            modelBuilder.Entity<CourseSession>()
                .HasMany(cs => cs.OrderItems)
                .WithOne(oi => oi.Session)
                .HasForeignKey(oi => oi.SessionId);

            // CourseSession - CartItem
            modelBuilder.Entity<CourseSession>()
                .HasMany(cs => cs.CartItems)
                .WithOne(ci => ci.Session)
                .HasForeignKey(ci => ci.SessionId);

            // CourseSession - CourseBooking
            modelBuilder.Entity<CourseSession>()
                .HasMany(cs => cs.Bookings)
                .WithOne(bo => bo.Session)
                .HasForeignKey(bo => bo.SessionId);

            // Order - PaymentMethod
            modelBuilder.Entity<Order>()
                .HasOne(o => o.PaymentMethod)
                .WithMany(pm => pm.Orders)
                .HasForeignKey(o => o.PaymentMethodId);

            // Order - Invoice
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Invoice)
                .WithOne(i => i.Order)
                .HasForeignKey<Invoice>(i => i.OrderId);

            // order - orderItem
            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            // cart - cartItem
            modelBuilder.Entity<Cart>()
                .HasMany(ca => ca.Items)
                .WithOne(ci => ci.Cart)
                .HasForeignKey(ci => ci.CartId);

            // setup property
            modelBuilder.Entity<CourseCategory>()
                .Property(cc => cc.Name)
                .HasConversion<string>();

            modelBuilder.Entity<PaymentMethod>()
                .Property(cc => cc.Name)
                .HasConversion<string>();

            // setup Constraint
            modelBuilder.Entity<Course>()
            .ToTable(c => c.HasCheckConstraint("CK_Course_Price_Positive", "[Price] >= 0"));

            modelBuilder.Entity<Cart>()
            .ToTable(c => c.HasCheckConstraint("CK_Cart_TotalPrice_Positive", "[TotalPrice] >= 0"));

            modelBuilder.Entity<Invoice>()
            .ToTable(c => c.HasCheckConstraint("CK_Invoice_TotalPrice_Positive", "[TotalPrice] >= 0"));

            modelBuilder.Entity<Order>()
            .ToTable(c => c.HasCheckConstraint("CK_Order_TotalPrice_Positive", "[TotalPrice] >= 0"));

            modelBuilder.Entity<OrderItem>()
            .ToTable(c => c.HasCheckConstraint("CK_OrderItem_Price_Positive", "[Price] >= 0"));

            // tanggal rental ga bisa di masa lalu
            modelBuilder.Entity<CourseSession>()
            .ToTable(c => c.HasCheckConstraint("CK_CourseSchedule_Date", "[Date] >= CAST(GETDATE() AS DATE)"));

        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            foreach (var entry in ChangeTracker.Entries<Invoice>()
            .Where(e => e.State == EntityState.Added))
            {
                var lastNumber = await Invoices.MaxAsync(i => (int?)i.InvoiceNumber, cancellationToken: cancellationToken) ?? 0;
                entry.Entity.InvoiceNumber = lastNumber + 1;
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}