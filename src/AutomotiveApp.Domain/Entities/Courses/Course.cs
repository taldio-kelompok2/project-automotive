using AutomotiveApp.Base.Entities;

namespace AutomotiveApp.Domain.Entities.Courses
{
    public class Course : BaseEntity
    {
        public required string Name { get; set; }
        public required uint Price { get; set; }
        public string? ImageFileName { get; set; }

        // Foreign Key
        public Guid? CategoryId { get; set; }

        // Navigation properties
        public virtual CourseCategory? Category { get; set; } = null!;
        public virtual ICollection<CourseSession> Sessions { get; set; } = [];

    }
}