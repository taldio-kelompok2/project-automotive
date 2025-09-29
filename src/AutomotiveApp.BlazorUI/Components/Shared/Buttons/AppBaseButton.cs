using AutomotiveApp.BlazorUI.Enums.Button;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace AutomotiveApp.BlazorUI.Components.Shared.Buttons
{
    public class AppBaseButton : MudButton
    {
        [Parameter]
        public ButtonWidth Width { get; set; } = ButtonWidth.Short;

        private string GetWidthCssClass()
        {
            return Width switch
            {
                ButtonWidth.Short => "custom-btn-short",
                ButtonWidth.Medium => "custom-btn-medium",
                ButtonWidth.Long => "custom-btn-long",
                _ => string.Empty
            };
        }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            Class = $"{Class} {GetWidthCssClass()}".Trim();
        }
    }
}