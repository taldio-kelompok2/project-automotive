namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseSessionQueryDto : BaseQueryDto, IDto
    {
        public required DateTime Date { get; set; }
        public required uint Capacity { get; set; }
        public required string Course { get; set; }
        public required Guid CourseId { get; set; }
    }

}