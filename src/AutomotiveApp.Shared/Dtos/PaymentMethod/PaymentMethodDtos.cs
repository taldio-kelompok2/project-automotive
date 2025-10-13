using System;
using AutomotiveApp.Shared.Enums;
using System.Text.Json.Serialization;

namespace AutomotiveApp.Application.PaymentMethods
{
    public class PaymentMethodCreateDto
    {
        public required string Name { get; set; }
        public bool Status { get; set; } = true;
    }

    public class PaymentMethodUpdateDto
    {
        public required string Name { get; set; }
        public bool Status { get; set; } = true;
    }

    public class PaymentMethodReadDto
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}