using Avalonia.Media;

namespace MetricsIntegrator.Style.Color
{
    public class ColorBrushFactory
    {
        //---------------------------------------------------------------------
        //		Constructor
        //---------------------------------------------------------------------
        private ColorBrushFactory()
        {
        }


        //---------------------------------------------------------------------
        //		Factories
        //---------------------------------------------------------------------
        public static IBrush Black()
        {
            return Color.Black.Brush;
        }

        public static IBrush Theme()
        {
            return Color.Theme.Brush;
        }

        public static IBrush ThemeAccent()
        {
            return Color.ThemeAccent.Brush;
        }

        public static IBrush White()
        {
            return Color.White.Brush;
        }
    }
}
