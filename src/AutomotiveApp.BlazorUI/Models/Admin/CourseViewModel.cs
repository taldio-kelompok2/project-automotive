using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class CourseViewModel
    {
        public CarCategory Category { get; set; } = CarCategory.SUV;
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!;
        public double Price { get; set; } = 0;
        public DateTime? Schedule { get; set; } = null;

        public Status Status { get; set; }
    }
}
