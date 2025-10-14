namespace AutomotiveApp.Shared.Dtos.Carts
{
    public class CartCreateDto : BaseCommandDto, IDto
    {
        public Guid UserId { get; set; }
    }
}
