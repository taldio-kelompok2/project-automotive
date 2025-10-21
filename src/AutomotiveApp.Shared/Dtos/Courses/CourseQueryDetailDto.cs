namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseQueryDetailDto : CourseQueryDto, IDto
    {
        public required string Description { get; set; }
        public Dictionary<Guid, DateTime> Sessions { get; set; } = [];
    }

}