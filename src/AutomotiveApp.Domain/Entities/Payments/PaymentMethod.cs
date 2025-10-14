using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Orders;
namespace AutomotiveApp.Domain.Entities.Payments
{
    public class PaymentMethod : BaseEntity
    {
        public string Name { get; set; } = null!;
        public bool Status { get; set; } = true;
        public string? ImageFileName { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = [];
    }
}