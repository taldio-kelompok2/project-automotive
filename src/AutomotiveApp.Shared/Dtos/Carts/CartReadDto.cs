using AutomotiveApp.Shared.Dtos.CartItems;
using AutomotiveApp.Shared.Dtos.Courses;

namespace AutomotiveApp.Shared.Dtos.Carts
{
    public class CartReadDto : BaseQueryDto, IDto
    {
        public long TotalPrice { get; set; }
        public List<CartItemReadDto> Items { get; set; } = [];
        public Guid UserId { get; set; }
    }
}
