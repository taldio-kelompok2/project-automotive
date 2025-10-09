namespace AutomotiveApp.Shared.Dtos.Carts
{
    public class CartReadDto
    {
        public Guid Id { get; set; }
        public long TotalPrice { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
