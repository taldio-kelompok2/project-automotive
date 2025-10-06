namespace AutomotiveApp.Shared.Dtos.Course
{
    public class CourseCategoryCommandDto : BaseCommandDto, IDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ImageFileName { get; set; }
    }


}