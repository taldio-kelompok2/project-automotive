using System.Text.Json.Serialization;
using AutomotiveApp.Domain.Enums;
using AutomotiveApp.Shared.Dtos.OrderItem;

namespace AutomotiveApp.Shared.Dtos.Order
{
    public class OrderCreateDto : BaseCommandDto, IDto
    {

        [JsonIgnore]
        public Guid UserId { get; set; }
        public Guid PaymentMethodId { get; set; }
        public OrderCreateDto()
        {
            Id = Guid.NewGuid();
        }
    }

    public class OrderUpdateDto : BaseCommandDto, IDto
    {
        public Guid PaymentMethodId { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OrderStatus Status { get; set; }
    }

    public class OrderReadDto : BaseQueryDto, IDto
    {
        public required long TotalPrice { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required OrderStatus Status { get; set; }
        public required Guid UserId { get; set; }
        public required Guid PaymentMethodId { get; set; }
        public required string PaymentMethod { get; set; }
    }

    public class OrderReadDetailsDto : BaseQueryDto, IDto
    {
        public required long TotalPrice { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public required OrderStatus Status { get; set; }
        public required Guid UserId { get; set; }
        public required Guid PaymentMethodId { get; set; }
        public required string PaymentMethod { get; set; }
        public required List<OrderItemReadDto> Items { get; set; }
    }
}
