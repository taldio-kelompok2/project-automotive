using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Courses;
using AutomotiveApp.Domain.Entities.Courses.Cart;
using AutomotiveApp.Domain.Entities.Orders;
using Microsoft.AspNetCore.Identity;

namespace AutomotiveApp.Domain.Entities.Auth
{
    public class User : IdentityUser<Guid>, IBaseEntity
    {
        public bool Status { get; set; } = true;
        public void MarkUpdated() => UpdatedAt = DateTime.UtcNow;
        public void UpdateStatus(bool status) => Status = status;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; private set; }

        // Refresh token untuk JWT authentication
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }

        // Navigation properties
        public virtual ICollection<Order> Orders { get; set; } = [];
        public virtual ICollection<CourseBooking> Bookings { get; set; } = [];
        public virtual Cart? Cart { get; set; }

    }
}