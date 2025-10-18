using AutomotiveApp.Base.Entities;

namespace AutomotiveApp.Domain.Entities.Courses
{
    public class CourseCategory : BaseEntity
    {
        public required string Name { get; set; }
        public required string Description { get; set; }
        public string? ImageFileName { get; set; }
        public string? HeroImageFileName { get; set; }
        public virtual ICollection<Course> Courses { get; set; } = [];
    }
}