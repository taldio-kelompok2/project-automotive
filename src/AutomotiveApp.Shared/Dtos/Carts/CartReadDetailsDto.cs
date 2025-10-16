

using AutomotiveApp.Shared.Dtos.CartItems;

namespace AutomotiveApp.Shared.Dtos.Carts
{
    public class CartReadDetailsDto : BaseQueryDto, IDto
    {
        public long TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public List<CartItemReadDto> Items { get; set; } = [];
    }
}
