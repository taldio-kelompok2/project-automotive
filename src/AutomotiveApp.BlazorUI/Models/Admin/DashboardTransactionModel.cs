namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class DashboardTransactionModel
    {
        public string? UserName { get; set; }
        public string Email { get; set; } = default!;
        public string InvoiceCode { get; set; } = default!; // OTO000XX
        public DateTime Date { get; set; } // date
        public string? PaymentMethodName { get; set; }
        public int TotalCourse { get; set; }
        public long TotalPrice { get; set; }
    }
}
