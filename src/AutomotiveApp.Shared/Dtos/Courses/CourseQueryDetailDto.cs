namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseQueryDetailDto : CourseQueryDto, IDto
    {
        public Dictionary<Guid, DateTime> Sessions { get; set; } = [];
    }

}