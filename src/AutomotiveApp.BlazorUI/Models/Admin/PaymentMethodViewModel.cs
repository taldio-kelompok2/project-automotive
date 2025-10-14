using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Components.Forms;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class PaymentMethodViewModel
    {
        public int ID { get; set; }
        public Guid PaymentMethodId { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; } = ""; 
        public Status Status { get; set; }

        public string? ImageFileName { get; set; }
        // public IFormFile? FileImageName { get; set; } 
        public IBrowserFile? File { get; set; }
    }
}