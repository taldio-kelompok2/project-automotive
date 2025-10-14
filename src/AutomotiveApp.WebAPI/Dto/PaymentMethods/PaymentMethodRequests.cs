using Microsoft.AspNetCore.Http;

namespace AutomotiveApp.WebAPI.Dto.PaymentMethods
{
    public sealed class PaymentMethodCreateForm
    {
        public required string Name { get; set; }
        public bool Status { get; set; } = true;
        public IFormFile? FileImageName { get; set; }
    }

    public class PaymentMethodUpdateRequest
    {
        public required string Name { get; set; }
        public bool Status { get; set; } = true;
        public IFormFile? FileImageName { get; set; } 
    }
}