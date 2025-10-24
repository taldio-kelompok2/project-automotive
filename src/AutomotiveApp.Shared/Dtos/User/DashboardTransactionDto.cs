using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomotiveApp.Shared.Dtos.User
{
    public class DashboardTransactionDto
    {
        public string UserName { get; set; }
        public string Email { get; set; } = default!;
        public string InvoiceCode { get; set; } = default!; // OTO000XX
        public DateTime CreatedAt { get; set; } // date
        public string PaymentMethodName { get; set; }
        public int CourseCount { get; set; }
        public long TotalPrice { get; set; }
    }
}
