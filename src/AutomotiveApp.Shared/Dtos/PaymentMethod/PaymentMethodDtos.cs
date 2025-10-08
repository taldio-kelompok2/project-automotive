using System;
using AutomotiveApp.Shared.Enums;
using System.Text.Json.Serialization;

namespace AutomotiveApp.Application.PaymentMethods
{
    public class PaymentMethodCreateDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionCategory Name { get; set; }         
        public bool Status { get; set; } = true;
    }

    public class PaymentMethodUpdateDto
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionCategory Name { get; set; }
        public bool Status { get; set; } = true;
    }

    public class PaymentMethodReadDto
    {
        public Guid Id { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransactionCategory Name { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}