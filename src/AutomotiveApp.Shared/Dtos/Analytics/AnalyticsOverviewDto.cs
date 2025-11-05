namespace AutomotiveApp.Shared.Dtos.Analytics
{
    public sealed class AnalyticsOverviewDto
    {
        public int CoursesCount { get; set; }
        public List<AnalyticsCardItemDto> Items { get; set; } = new();
    }

    public sealed class AnalyticsCardItemDto
    {
        public string Key { get; set; } = "courses";
        public string Label { get; set; } = "Courses";
        public int Value { get; set; }
        public string Suffix { get; set; } = "+";
    }
}
