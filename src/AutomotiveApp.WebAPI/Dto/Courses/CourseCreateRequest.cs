using AutomotiveApp.Shared.Dtos;

namespace AutomotiveApp.WebAPI.Dto.Courses
{
    public class CourseCreateRequest : BaseCommandDto, ICommandDto
    {
        public required string Name { get; set; }
        public string? Description { get; set; }
        public required int Price { get; set; }
        public required Guid CategoryId { get; set; }
        public IFormFile? Image { get; set; }

        public CourseCreateRequest()
        {
            Id = Guid.NewGuid();
        }
    }
}