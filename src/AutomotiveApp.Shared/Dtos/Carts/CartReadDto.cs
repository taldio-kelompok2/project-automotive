namespace AutomotiveApp.Shared.Dtos.Carts
{
    public class CartReadDto : BaseQueryDto, IDto
    {
        public long TotalPrice { get; set; }
        public Guid UserId { get; set; }

    }
}
