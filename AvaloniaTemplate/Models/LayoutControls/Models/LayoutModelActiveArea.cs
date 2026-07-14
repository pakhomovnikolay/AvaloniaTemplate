using Avalonia;
using Avalonia.Media;

namespace AvaloniaTemplate.Models.LayoutControls.Models
{
    public class LayoutModelActiveArea
    {
        public Rect? Area { get; set; }
        public Pen? RectPen { get; set; }
        public IBrush? RectFill { get; set; }
    }
}
