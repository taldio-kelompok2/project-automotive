using System;

namespace AutomotiveApp.Shared.Dtos.OrderItem
{
    public class OrderItemCreateDto
    {
        public long Price { get; set; }
        public Guid OrderId { get; set; }
        public Guid SessionId { get; set; }
    }

    public class OrderItemUpdateDto
    {
        public long Price { get; set; }
        public Guid SessionId { get; set; }
    }

    public class OrderItemReadDto
    {
        public Guid Id { get; set; }
        public long Price { get; set; }
        public Guid OrderId { get; set; }
        public Guid SessionId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
