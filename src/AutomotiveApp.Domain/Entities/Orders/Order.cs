using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Auth;
using AutomotiveApp.Domain.Entities.Invoices;
using AutomotiveApp.Domain.Entities.Payments;
using AutomotiveApp.Domain.Enums;

namespace AutomotiveApp.Domain.Entities.Orders
{
    public class Order : BaseEntity
    {
        public long TotalPrice { get; set; } = 0;
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Foreign Keys
        public Guid UserId { get; set; }
        public Guid PaymentMethodId { get; set; }

        // Navigation properties
        public virtual PaymentMethod PaymentMethod { get; set; } = null!;
        public virtual Invoice? Invoice { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
        public virtual User User { get; set; } = null!;
    }
}