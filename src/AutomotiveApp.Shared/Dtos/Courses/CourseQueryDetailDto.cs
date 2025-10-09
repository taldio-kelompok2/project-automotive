namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseQueryDetailDto : CourseQueryDto, IDto
    {
        public List<DateTime> Sessions { get; set; } = [];
    }

}