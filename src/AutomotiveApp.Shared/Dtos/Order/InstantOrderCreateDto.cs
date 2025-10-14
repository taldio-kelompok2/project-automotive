using System.Text.Json.Serialization;

namespace AutomotiveApp.Shared.Dtos.Order
{
    public class InstantOrderCreateDto : BaseCommandDto, IDto
    {
        public Guid PaymentId { get; set; }
        public Guid SessionId { get; set; }

        [JsonIgnore]
        public Guid UserId { get; set; }
    }
}