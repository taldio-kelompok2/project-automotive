using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.CartItems
{
    public class CartItemCreateDto
    {
        [Required(ErrorMessage = "Cart Id Required")]
        public Guid CartId { get; set; }

        [Required(ErrorMessage = "Session Id Required")]
        public Guid SessionId { get; set; }
    }

}
