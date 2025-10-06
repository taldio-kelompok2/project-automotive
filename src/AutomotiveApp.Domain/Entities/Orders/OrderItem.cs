// using AutomotiveApp.Base.Entities;
// using AutomotiveApp.Domain.Entities.Courses;

// namespace AutomotiveApp.Domain.Entities.Orders
// {
//     public class OrderItem : BaseEntity
//     {
//         public required uint Price { get; set; }

//         // Foreign Key
//         public Guid OrderId { get; set; }
//         public Guid SessionId { get; set; }

//         // Navigation properties
//         public virtual Order Order { get; set; } = null!;
//         public virtual CourseSession Session { get; set; } = null!;
//     }
// }

using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Entities.Orders;
using System.ComponentModel.DataAnnotations.Schema;

namespace AutomotiveApp.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity
    {
        // public required uint Price { get; set; }
        public required long Price { get; set; }  

        // Foreign Keys
        public Guid OrderId { get; set; }
        public Guid SessionId { get; set; }

        // Navigation properties
        [ForeignKey(nameof(OrderId))]
        public virtual Order Order { get; set; } = null!;

        [ForeignKey(nameof(SessionId))]
        public virtual CourseSession Session { get; set; } = null!;
    }
}
