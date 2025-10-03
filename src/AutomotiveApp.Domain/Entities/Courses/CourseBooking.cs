using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Auth;

namespace AutomotiveApp.Domain.Entities.Courses
{
    public class CourseBooking : BaseEntity
    {
        //Foreign Key
        public Guid UserId { get; set; }
        public Guid SessionId { get; set; }

        ////Navigation Properties
        public virtual User User { get; set; } = null!;
        public virtual CourseSession Session { get; set; } = null!;
    }
}