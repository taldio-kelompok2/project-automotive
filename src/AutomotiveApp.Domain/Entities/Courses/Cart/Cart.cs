using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Auth;

namespace AutomotiveApp.Domain.Entities.Courses.Cart
{
    public class Cart : BaseEntity
    {
        public required long TotalPrice { get; set; } = 0;

        // Foreign Keys
        public Guid UserId { get; set; }

        // Navigation properties
        public virtual ICollection<CartItem> Items { get; set; } = [];
        public virtual User User { get; set; } = null!;
    }
}