using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.CartItems
{
    public class CartItemCreateDto : BaseCommandDto, IDto
    {
        public required Guid CartId { get; set; }
        public required Guid SessionId { get; set; }
    }

}
