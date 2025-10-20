namespace AutomotiveApp.Shared.Dtos.Courses
{
    public class CourseCategoryQueryDto : BaseQueryDto, IDto
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ImageUrl { get; set; }
        public string? HeroImageUrl { get; set; }
    }


}