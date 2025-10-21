using AutomotiveApp.Shared.Dtos;
using Swashbuckle.AspNetCore.Annotations;

namespace AutomotiveApp.WebAPI.Dto.Courses
{
    public class CourseEditRequest : BaseCommandDto, ICommandDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public int? Price { get; set; }
        public Guid? CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}