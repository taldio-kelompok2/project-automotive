using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

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

        [Range(1000, int.MaxValue, ErrorMessage = "Price must be at least 1000.")]
        internal int Price { get; set; } = 1000;
        public string CoursePrice => string.Format(new CultureInfo("id-ID"), "IDR {0:N0}", Price);
        internal Guid? CategoryId { get; set; }
        public string Category { get; set; } = "-";
        internal string Image { get; set; } = "";
        internal IBrowserFile? File { get; set; }
        internal DateTime? Schedule { get; set; } = null;
        internal uint Capacity { get; set; } = 0;
        internal Status Status { get; set; }

        // for edit / add sessions
        internal List<CourseSessionVM> Sessions { get; set; } = new();

        // for dialog view
        public string Session { get; set; } = "-";
    }
}