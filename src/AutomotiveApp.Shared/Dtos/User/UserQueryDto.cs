namespace AutomotiveApp.Shared.Dtos.User
{
    public class UserQueryDto
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public int OrdersCount { get; set; }
        public int BookingsCount { get; set; }
        public bool HasCart { get; set; }

        // nanti ganti ke dto order/booking/cart
        //public virtual ICollection<Order> Orders { get; set; } = [];
        //public virtual ICollection<CourseBooking> Bookings { get; set; } = [];
        //public virtual Cart? Cart { get; set; }
    }
}
