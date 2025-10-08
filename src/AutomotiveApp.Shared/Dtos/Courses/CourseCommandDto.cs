namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseCommandDto : BaseCommandDto, IDto
    {
        public required string Name { get; set; }
        public required uint Price { get; set; }
        public required Guid CategoryId { get; set; }
        public string? ImageFilename { get; set; }
    }


}