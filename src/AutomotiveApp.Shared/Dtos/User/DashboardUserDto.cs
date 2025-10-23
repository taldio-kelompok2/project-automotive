using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomotiveApp.Shared.Dtos.User
{
    public class DashboardUserDto
    {
        public string Email { get; set; } = default!;
        public string InvoiceCode { get; set; } = default!; // OTO000XX
        public DateTime CreatedAt { get; set; } // date
        public int CourseCount { get; set; }
        public long TotalPrice { get; set; }

        // details?
        public string PaymentMethodName { get; set; }
        public Guid UserId { get; set; } 
        public string UserName { get; set; }
    }
}
