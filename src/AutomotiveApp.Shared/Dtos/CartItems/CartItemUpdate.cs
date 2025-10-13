namespace AutomotiveApp.Shared.Dtos.CartItems
{
    public class CartItemUpdateDto : BaseCommandDto, IDto
    {
        public Guid SessionId { get; set; }
    }
}
