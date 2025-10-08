namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseSessionCommandDto : BaseCommandDto, IDto
    {
        public required DateTime Date { get; set; }
        public required uint Capacity { get; set; }
        public required Guid CourseId { get; set; }
    }

}