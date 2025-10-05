using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Domain.Entities.Orders;

namespace AutomotiveApp.Domain.Entities.Courses
{
    public class CourseSession : BaseEntity
    {
        public required DateTime Date { get; set; }
        public required uint Capacity { get; set; }

        // Foreign Key
        public Guid CourseId { get; set; }

        //Navigation Properties
        public virtual Course Course { get; set; } = null!;
        public virtual ICollection<CourseBooking> Bookings { get; set; } = [];
        public virtual ICollection<OrderItem> OrderItems { get; set; } = [];
        public virtual ICollection<CartItem> CartItems { get; set; } = [];
    }
}