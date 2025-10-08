namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseCategoryCommandDto : BaseCommandDto, IDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ImageFileName { get; set; }
    }

}