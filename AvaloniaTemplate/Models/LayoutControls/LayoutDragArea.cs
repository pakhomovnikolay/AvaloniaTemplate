using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Models.LayoutControls.Models;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutDragArea : Control
    {
        public LayoutModelFrameDragArea? Area { get; set; }
        public override void Render(DrawingContext context)
        {
            RenderDragArea(context);
        }

        #region Отрисовать границы изменения размера ячеек
        /// <summary>
        /// Отрисовать границы изменения размера ячеек
        /// </summary>
        /// <param name="context"></param>
        private void RenderDragArea(DrawingContext context)
        {
            if (Area is null)
                return;

            var startLeft = Area.Start.Value.Left;
            var startTop = Area.Start.Value.Top;
            var startRight = Area.Start.Value.Right;
            var startBottom = Area.Start.Value.Bottom;

            var endLeft = Area.End.Value.Left;
            var endTop = Area.End.Value.Top;
            var endRight = Area.End.Value.Right;
            var endBottom = Area.End.Value.Bottom;

            if (Area.Flow == Orientation.Vertical)
            {
                var pen = new Pen(Area.BorderBrush, Area.Start.Value.Width);
                context.DrawLine(pen, new Point(startLeft, startTop), new Point(startLeft, startBottom));

                pen = new Pen(Area.BorderBrush, Area.End.Value.Width);
                context.DrawLine(pen, new Point(endLeft, endTop), new Point(endLeft, endBottom));
            }
            else
            {
                var pen = new Pen(Area.BorderBrush, Area.Start.Value.Height);
                context.DrawLine(pen, new Point(startLeft, startTop), new Point(startRight, startTop));

                pen = new Pen(Area.BorderBrush, Area.End.Value.Height);
                context.DrawLine(pen, new Point(endLeft, endTop), new Point(endRight, endTop));
            }
        }
        #endregion
    }
}
