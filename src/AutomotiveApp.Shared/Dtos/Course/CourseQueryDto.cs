using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.Shared.Dtos.Course
{
    public class CourseQueryDto
    {
        [Required(ErrorMessage = "Name is required")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(1, int.MaxValue, ErrorMessage = "Price must not be less then 0")]
        public required uint Price { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public required string Category { get; set; }
    }


}