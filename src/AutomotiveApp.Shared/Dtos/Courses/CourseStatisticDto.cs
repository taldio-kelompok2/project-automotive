using AutomotiveApp.Shared.Dtos;

namespace AutomotiveApp.WebAPI.Dto.Courses
{
    public class CourseStatisticDto : IDto
    {
        public Guid CourseId { get; set; }
        public string CourseTitle { get; set; } = string.Empty;
        public string? Category { get; set; }

        public int TotalOrders { get; set; }
        public int TotalStudents { get; set; }
        public long TotalRevenue { get; set; }
        public double AverageOrderValue => TotalOrders > 0 ? (double)TotalRevenue / TotalOrders : 0;

        public List<MonthlySalesDto> MonthlySales { get; set; } = [];
        public List<DailySalesDto> DailySales { get; set; } = [];
    }

    public class MonthlySalesDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public long TotalRevenue { get; set; }
        public int TotalOrders { get; set; }

        public string MonthName => new DateTime(Year, Month, 1).ToString("MMMM yyyy");
    }

    public class DailySalesDto
    {
        public DateTime Date { get; set; }
        public long TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
    }

}