namespace AutomotiveApp.Shared.Dtos.Courses
{

    public class CourseBookingCommandDto : BaseCommandDto, IDto
    {
        public required Guid UserId { get; set; }
        public required Guid SessionId { get; set; }
    }
}