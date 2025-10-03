using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Courses;

namespace AutomotiveApp.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity
    {
        public required uint Price { get; set; }

        // Foreign Key
        public Guid OrderId { get; set; }
        public Guid SessionId { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual CourseSession Session { get; set; } = null!;
    }
}