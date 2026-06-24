using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using System;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutTextPositionType : Control
    {
        public VerticalAlignment VerticalPosition { get; set; } = VerticalAlignment.Stretch;
        public HorizontalAlignment HorizontalPosition { get; set; } = HorizontalAlignment.Stretch;
        public Orientation Position { get; set; } = Orientation.Vertical;

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            var pen = new Pen(Brushes.Black, 1);

            if (Position == Orientation.Vertical)
            {
                var lines = new[]
                {
                    Bounds.Width * 0.5,
                    Bounds.Width * 0.7,
                    Bounds.Width * 0.5,
                    Bounds.Width * 0.7
                };

                if (Bounds.Height <= 30)
                {
                    lines =
                    [
                        Bounds.Width * 0.5,
                        Bounds.Width * 0.7,
                        Bounds.Width * 0.5
                    ];
                }

                var step = 4.0;

                var blockHeight = (lines.Length - 1) * step;

                double startY = VerticalPosition switch
                {
                    VerticalAlignment.Top => step,
                    VerticalAlignment.Center => (Bounds.Height - blockHeight - 1) / 2,
                    VerticalAlignment.Bottom => Bounds.Height - blockHeight - step - 1,
                    _ => step
                };

                for (var i = 0; i < lines.Length; i++)
                {
                    var width = lines[i];
                    var x = Math.Round((Bounds.Width - width) / 2) + 0.5;
                    var y = Math.Round(startY + i * step) + 0.5;
                    context.DrawLine(
                        pen,
                        new Point(x, y),
                        new Point(x + width, y));
                }
            }
            else
            {
                var lines =
                new[]
                {
                    Bounds.Width * 0.5,
                    Bounds.Width * 0.7,
                    Bounds.Width * 0.5,
                    Bounds.Width * 0.7,
                    Bounds.Width * 0.5
                };
                if (Bounds.Height <= 30)
                {
                    lines =
                    [
                        Bounds.Width * 0.5,
                        Bounds.Width * 0.7,
                        Bounds.Width * 0.5,
                        Bounds.Width * 0.7,
                    ];
                }

                var step = 4.0;

                var blockWidth = (lines.Length - 1) * step;
                double startY = (Bounds.Height - blockWidth) / 2;

                for (var i = 0; i < lines.Length; i++)
                {
                    var width = lines[i];
                    double startX = HorizontalPosition switch
                    {
                        HorizontalAlignment.Left => step,
                        HorizontalAlignment.Center => (Bounds.Width - width) / 2,
                        HorizontalAlignment.Right => Bounds.Width - width - step,
                        _ => step
                    };
                    var x = Math.Round(startX) + 0.5;
                    var y = Math.Round(startY + i * step) + 0.5;
                    context.DrawLine(pen, new Point(x, y), new Point(x + width, y));
                }
            }
        }
    }
}
