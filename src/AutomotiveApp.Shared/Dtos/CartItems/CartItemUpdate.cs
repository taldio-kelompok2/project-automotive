using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.CartItems
{
    public class CartItemUpdateDto
    {
        [Required(ErrorMessage = "Session Id Required")]
        public Guid SessionId { get; set; }
    }
}
