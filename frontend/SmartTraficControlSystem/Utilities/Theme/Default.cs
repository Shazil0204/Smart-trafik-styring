using MudBlazor;

namespace SmartTraficControlSystem.Utilities.Theme
{
    public class DefaultTheme : MudTheme
    {
        public DefaultTheme()
        {
            PaletteLight = new()
            {
                Primary = "#2A2C24",
                Secondary = "#575A4B",
                Tertiary = "#816C61",


            };

            LayoutProperties = new LayoutProperties()
            {
                DefaultBorderRadius = "4px"
            };

        }
    }
}
