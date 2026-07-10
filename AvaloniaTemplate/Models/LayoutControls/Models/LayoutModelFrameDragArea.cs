using Avalonia;
using Avalonia.Layout;
using Avalonia.Media;

namespace AvaloniaTemplate.Models.LayoutControls.Models
{
    public class LayoutModelFrameDragArea
    {
        public Rect? Start { get; set; }
        public Rect? End { get; set; }
        public Orientation? Flow { get; set; }
        public IBrush BorderBrush { get; set; }
    }
}
