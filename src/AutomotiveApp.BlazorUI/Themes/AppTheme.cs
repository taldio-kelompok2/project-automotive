using MudBlazor;

namespace AutomotiveApp.BlazorUI.Themes
{
    public class AppTheme : MudTheme
    {
        public AppTheme()
        {
            Typography = new Typography
            {
                Default = new DefaultTypography
                {
                    FontFamily = ["Montserrat", "Helvetica", "Arial", "sans-serif"],
                    FontSize = "1rem",
                    FontWeight = "400",
                    LineHeight = "1",
                    LetterSpacing = "normal",
                },

                H6 = new H6Typography
                {
                    FontSize = "1.25rem",
                    FontWeight = "400",
                },

                H5 = new H5Typography
                {
                    FontSize = "1.5rem",
                    FontWeight = "500",
                },

                H4 = new H4Typography
                {
                    FontSize = "1.75rem",
                    FontWeight = "500",
                },

                H3 = new H3Typography
                {
                    FontSize = "2rem",
                    FontWeight = "600",
                },

                H2 = new H2Typography
                {
                    FontSize = "2.5rem",
                    FontWeight = "600",
                },

                H1 = new H1Typography
                {
                    FontSize = "3rem",
                    FontWeight = "600",
                },

                Body1 = new Body1Typography
                {
                    FontSize = "1rem",
                    FontWeight = "500",
                },

                Body2 = new Body2Typography
                {
                    FontSize = "1rem",
                    FontWeight = "400",
                }
            };

            PaletteLight = new PaletteLight
            {
                Black = "#000000",
                White = "#FFFFFF",

                Primary = "#790B0A",
                Secondary = "#828282",
                Tertiary = "#5596f0",

                Background = "#ffffff",
                TextPrimary = "#333333",
                TextSecondary = "#4F4F4F",
                Surface = "#f5f5f5",
                AppbarBackground = "#ffffff",
                AppbarText = "#333333",

                Success = "#4CAF50",
                Info = "#2196F3",
                Warning = "#FFC107",
                Error = "#F44336"
            };

            PaletteDark = new PaletteDark
            {
                Black = "#000000",
                White = "#FFFFFF",
                Primary = "#43566aff",
                Secondary = "#c5b858",
                Tertiary = "#1b5e20",
                Success = "#00FFFF",
                Info = "#FFFF00",
                Warning = "#FF00FF",
                Error = "#C0C0C0",
                Dark = "#303030",
                Background = "#303030"
            };
        }
    }
}
