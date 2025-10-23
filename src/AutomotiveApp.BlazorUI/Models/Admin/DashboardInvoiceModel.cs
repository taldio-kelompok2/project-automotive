namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class DashboardInvoiceModel
    {
        public string Email { get; set; } = default!;
        public string InvoiceCode { get; set; } = default!; // OTO000XX
        public DateTime Date { get; set; } // date
        public int TotalCourse { get; set; }
        public long TotalPrice { get; set; }

        // details?
        public string PaymentMethodName { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
    }
}
