using AutomotiveApp.Shared.Dtos;

namespace AutomotiveApp.BlazorUI.Models.Course
{
    public class UpdateCourseViewModel : BaseCommandDto, IDto
    {
        public string? Name { get; set; }
        public int? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}