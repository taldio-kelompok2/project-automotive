using System;

namespace AutomotiveApp.Application.Invoices
{
    public class InvoiceReadDto
    {
        public Guid Id { get; set; }
        public long TotalPrice { get; set; }
        public int InvoiceNumber { get; set; }
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
    
    public class InvoiceCreateDto
    {
        public Guid OrderId { get; set; }
    }
}
