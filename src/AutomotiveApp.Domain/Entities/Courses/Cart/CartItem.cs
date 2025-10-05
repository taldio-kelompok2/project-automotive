using AutomotiveApp.Base.Entities;

namespace AutomotiveApp.Domain.Entities.Courses.Cart
{
    public class CartItem : BaseEntity
    {
        //Foreign Keys
        public Guid CartId { get; set; }
        public Guid SessionId { get; set; }

        // Navigation properties
        public virtual Cart Cart { get; set; } = null!;
        public virtual CourseSession Session { get; set; } = null!;

    }
}