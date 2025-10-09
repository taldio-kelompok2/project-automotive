using AutomotiveApp.Shared.Dtos;

namespace AutomotiveApp.WebAPI.Dto.Courses
{
    public class CourseCategoryCreateRequest : BaseCommandDto, ICommandDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public IFormFile? Image { get; set; }

        public CourseCategoryCreateRequest()
        {
            Id = Guid.NewGuid();
        }
    }
}