using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;
using Microsoft.AspNetCore.Components.Forms;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class CategoryViewModel
    {
        public int ID { get; set; }
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public string Logo { get; set; } = "";
        public string Hero { get; set; } = "";
        public Status Status { get; set; }

        public string? ImageFileName { get; set; }
        public IBrowserFile? File { get; set; }
        public IBrowserFile? HeroFile { get; set; }
    }
}