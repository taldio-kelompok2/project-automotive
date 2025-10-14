namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseBookingQueryDto : BaseCommandDto, IDto
    {
        public required DateTime Session { get; set; }
        public required string Course { get; set; }
        public required string UserId { get; set; }
        public required string SessionId { get; set; }
    }

}