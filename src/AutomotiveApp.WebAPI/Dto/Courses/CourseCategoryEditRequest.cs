using AutomotiveApp.Shared.Dtos;

namespace AutomotiveApp.WebAPI.Dto.Courses
{
    public class CourseCategoryEditRequest : BaseCommandDto, ICommandDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public IFormFile? Image { get; set; }
    }
}