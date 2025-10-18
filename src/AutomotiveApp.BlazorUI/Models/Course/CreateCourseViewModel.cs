using AutomotiveApp.Shared.Dtos;

namespace AutomotiveApp.BlazorUI.Models.Course
{
    public class CreateCourseViewModel : BaseCommandDto, IDto
    {
        public required string Name { get; set; }
        public required int Price { get; set; }
        public required Guid CategoryId { get; set; }
        public IFormFile? Image { get; set; }

        public CreateCourseViewModel()
        {
            Id = Guid.NewGuid();
        }
    }
}