using Avalonia.Controls;
using AvaloniaTemplate.Models.LayoutControls;

namespace AvaloniaTemplate.Models.Enums
{
    public class GridStyleType
    {
        public static Control CreateGrid(
            double width,
            double height,
            BorderStyleType Bottom = BorderStyleType.None,
            BorderStyleType Top = BorderStyleType.None,
            BorderStyleType Left = BorderStyleType.None,
            BorderStyleType Right = BorderStyleType.None,
            BorderStyleType InsideHorizontal = BorderStyleType.None,
            BorderStyleType InsideVertical = BorderStyleType.None)
        {

            var control = new LayoutGridStyle()
            {
                Bottom = Bottom,
                Top = Top,
                Left = Left,
                Right = Right,
                InsideHorizontal = InsideHorizontal,
                InsideVertical = InsideVertical,
                Width = width,
                Height = height
            };
            control.InvalidateVisual();
            return control;
        }
    }
}
