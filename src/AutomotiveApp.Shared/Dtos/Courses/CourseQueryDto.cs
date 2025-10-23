namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseQueryDto : BaseQueryDto, IDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Price { get; set; }
        public string? ImageUrl { get; set; }
        public string? Category { get; set; }
        public Guid CategoryId { get; set; }
    }

}