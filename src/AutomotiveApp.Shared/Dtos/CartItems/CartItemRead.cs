using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.CartItems
{
    public class CartItemReadDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
