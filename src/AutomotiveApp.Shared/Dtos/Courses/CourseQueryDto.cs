namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseQueryDto : BaseQueryDto, IDto
    {
        public required string Name { get; set; }
        public required int Price { get; set; }
        public string? ImageFileName { get; set; }
        public required string Category { get; set; }
    }


}