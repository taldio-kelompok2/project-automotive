using AutomotiveApp.Base.Entities;
using AutomotiveApp.Domain.Entities.Orders;

namespace AutomotiveApp.Domain.Entities.Invoices
{
    public class Invoice : BaseEntity
    {
        private static readonly string INVOICE_CODE_HEADER = "OTO";
        public int InvoiceNumber { get; set; }
        public required uint TotalPrice { get; set; }

        //Foreign Key
        public Guid OrderId { get; set; }

        //Navigation Properties
        public virtual Order Order { get; set; } = null!;

        public string? GetinvoiceCode()
        {
            return $"{INVOICE_CODE_HEADER}-{InvoiceNumber}";
        }

    }
}