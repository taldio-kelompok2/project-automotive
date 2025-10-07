using System;
using AutomotiveApp.Shared.Enums;
using System.Text.Json.Serialization;

namespace AutomotiveApp.Application.Orders
{
    public class OrderCreateDto
    {
        public Guid UserId { get; set; }
        public Guid PaymentMethodId { get; set; }  
        [JsonConverter(typeof(JsonStringEnumConverter))]      
        public OrderStatus Status { get; set; }  
    }

    public class OrderUpdateDto
    {
        public Guid PaymentMethodId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
    }

    public class OrderReadDto
    {
        public Guid Id { get; set; }
        public long TotalPrice { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
        public Guid UserId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
