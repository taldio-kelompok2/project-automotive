
// using AutomotiveApp.Application.Orders;
using AutomotiveApp.Shared.Dtos.Order;
using AutomotiveApp.Shared.Dtos.Carts;
using AutomotiveApp.Shared.Dtos.Courses;
using AutomotiveApp.Shared.Dtos.Order;

namespace AutomotiveApp.Shared.Dtos.User
{
    public class UserProfileDto : IDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = [];

        public ICollection<OrderReadDto>? Orders { get; set; } = [];
        public ICollection<CourseBookingQueryDto>? Bookings { get; set; } = [];
        public CartReadDto? Cart { get; set; }
    }
}
