using AutomotiveApp.BlazorUI.Enums;
using AutomotiveApp.Shared.Enums;

namespace AutomotiveApp.BlazorUI.Models.Admin
{
    public class CategoryViewModel
    {
        public string Name { get; set; }
        public Status Status { get; set; } = Status.Active; // default

    }
}
