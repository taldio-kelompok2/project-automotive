namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseSessionEditCommandDto : BaseCommandDto, IDto
    {
        public DateTime Date { get; set; }
        public uint Capacity { get; set; }
        public Guid CourseId { get; set; }
    }

}