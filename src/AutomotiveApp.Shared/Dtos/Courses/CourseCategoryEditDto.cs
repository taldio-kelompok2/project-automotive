namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseCategoryEditDto : BaseCommandDto, IDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? ImageFilename { get; set; }
    }

}