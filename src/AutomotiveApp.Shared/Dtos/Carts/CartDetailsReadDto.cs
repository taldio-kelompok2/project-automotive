using AutomotiveApp.Domain.Entities.Courses.Cart;

namespace AutomotiveApp.Shared.Dtos.Carts
{
    public class CartDetailsReadDto : BaseQueryDto, IDto
    {
        public long TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public List<CartDetailsReadDto> Items { get; set; } = [];
    }
}
