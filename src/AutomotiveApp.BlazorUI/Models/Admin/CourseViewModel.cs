using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class CourseSessionVM
    {
        public Guid? Id { get; set; }
        public DateTime? Date { get; set; }  
        public uint Capacity { get; set; }
    }

    public class CourseViewModel
    {
        [Browsable(false)]
        [Display(AutoGenerateField = false)]
        internal int ID { get; set; }
        internal Guid CourseId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public int Price { get; set; } = 0;
        internal Guid CategoryId { get; set; }
        public string Category { get; set; } = "-";
        internal string Image { get; set; } = "";
        internal IBrowserFile? File { get; set; }
        public DateTime? Schedule { get; set; } = null;
        public uint Capacity { get; set; } = 0;
        internal Status Status { get; set; }

        internal List<CourseSessionVM> Sessions { get; set; } = new();
    }
}