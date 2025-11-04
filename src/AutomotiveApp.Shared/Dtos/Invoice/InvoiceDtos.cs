using System;
using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Application.Invoices
{
    public class InvoiceReadDto
    {
        public Guid Id { get; set; }
        public long TotalPrice { get; set; }
        public int InvoiceNumber { get; set; }
        public string InvoiceCode { get; set; } = default!;
        public Guid OrderId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class InvoiceCreateFormDto
    {
        [Required]
        public Guid OrderId { get; set; }
    }
    
    public class InvoiceUpdateFormDto
    {
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Invoice Number Not Found")]
        public int InvoiceNumber { get; set; }  

        [Required]
        [Range(0, long.MaxValue, ErrorMessage = "Total Price Not Negative Number")]
        public long TotalPrice { get; set; }
    }

    public class InvoiceDetailsDto
    {
        public Guid Id { get; set; }
        public string InvoiceCode { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public long TotalPrice { get; set; }
        public string? PaymentMethod { get; set; }
        public string CustomerName { get; set; } = "-";   
        public string CustomerEmail { get; set; } = "-"; 
        public List<InvoiceItemDto> Items { get; set; } = new();
    }

    public class InvoiceItemDto
    {
        public string CourseName { get; set; } = "";   
        public string Type { get; set; } = "";        
        public DateTime Schedule { get; set; }        
        public decimal Price { get; set; }             
    }

}
