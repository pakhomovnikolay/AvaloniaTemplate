using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Models.LayoutControls.Models
{
    public class LayoutModelFrameLine
    {
        public double PositionX { get; set; }
        public double PositionY { get; set; }
        public double Right { get; set; }
        public double Bottom { get; set; }
        public double Size { get; set; }
        public Pen? LinePen { get; set; }
    }
}
