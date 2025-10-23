using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Components.Forms;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class CourseViewModel
    {
        public int ID { get; set; }
        public Guid CourseId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Price { get; set; } = 0;
        public Guid CategoryId { get; set; }
        public string Category { get; set; } = "-";
        public string Image { get; set; } = "";
        public IBrowserFile? File { get; set; }
        public DateTime? Schedule { get; set; } = null;
        public uint Capacity { get; set; } = 0;
        public Status Status { get; set; }
    }
}