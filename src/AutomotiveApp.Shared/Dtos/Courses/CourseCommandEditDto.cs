namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseCommandEditDto : BaseCommandDto, IDto
    {
        public string? Name { get; set; }
        public uint? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public string? ImageFilename { get; set; }
    }

}