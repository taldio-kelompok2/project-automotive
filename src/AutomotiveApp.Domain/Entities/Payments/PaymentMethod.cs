using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Orders;
using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.Domain.Entities.Payments
{
    public class PaymentMethod : BaseEntity
    {
        public TransactionCategory Name { get; set; }

        public bool Status { get; set; } = true;

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}