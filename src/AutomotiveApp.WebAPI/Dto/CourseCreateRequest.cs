using AutomotiveApp.Base.Entities;
using AutomotiveApp.Shared.Dtos;

namespace AutomotiveApp.WebAPI.Dto
{
    public class CourseCreateRequest : BaseCommandDto, ICommandDto
    {
        public required string Name { get; set; }
        public required int Price { get; set; }
        public required Guid CategoryId { get; set; }
        public IFormFile? Image { get; set; }
    }
}