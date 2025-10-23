using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutomotiveApp.Shared.Dtos.User
{
    public class DashboardDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalPayments { get; set; }
        public int TotalCourses { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}
