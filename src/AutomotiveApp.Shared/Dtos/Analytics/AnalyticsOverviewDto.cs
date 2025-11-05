namespace AutomotiveApp.Shared.Dtos.Analytics
{
    public sealed class AnalyticsOverviewDto
    {
        public int CoursesCount { get; set; }
        public int OrdersCount { get; set; }
        public int PartnersCount { get; set; }
        public List<AnalyticsCardItemDto> Items { get; set; } = new();
    }

    public sealed class AnalyticsCardItemDto
    {
        public string Label { get; set; } = "";
        public int Value { get; set; }
        public string Suffix { get; set; } = "+";
    }

}
