using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class PaymentMethodViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public  string Logo { get; set; }

        public Status Status { get; set; }
    }
}
