using AutomotiveApp.Shared.Dtos.Courses;

namespace AutomotiveApp.Shared.Dtos.CartItems
{
    public class CartItemReadDto : BaseQueryDto, IDto
    {
        public Guid SessionId { get; set; }
        public DateTime Schedule { get; set; }
        public required CourseQueryDto Course { get; set; }
    }
}
