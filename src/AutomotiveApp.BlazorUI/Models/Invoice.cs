namespace AutomotiveApp.BlazorUI.Models;

public class Invoice
{
    public required string InvoiceNumber { get; set; }
    public required string Date { get; set; }
    public required string TotalPrice { get; set; }
}
