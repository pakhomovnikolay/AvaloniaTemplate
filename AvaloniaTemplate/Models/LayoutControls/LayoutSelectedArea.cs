using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Models.LayoutControls.Models;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutSelectedArea : Control
    {
        public LayoutModelActiveArea? SelectedArea { get; set; }
        public override void Render(DrawingContext context)
        {
            RenderAnchorArea(context);
        }

        #region Отрисовать область начальной ячейки
        /// <summary>
        /// Отрисовать область начальной ячейки
        /// </summary>
        /// <param name="context"></param>
        private void RenderAnchorArea(DrawingContext context)
        {
            if (SelectedArea is null)
                return;

            var rect = SelectedArea.Area ?? new();
            context.DrawRectangle(SelectedArea.RectFill, SelectedArea.RectPen, rect);

            //Rect rectIn = new(rect.Left + 2, rect.Top + 2, rect.Width - 4, rect.Height - 4);
            //var RectPen = new Pen(Helper.GetColor(Helper.GetAutoHighlight(AnchorArea.RectPen.Brush, 0.05)), 1);
            //context.DrawRectangle(AnchorArea.RectFill, RectPen, rectIn);
        }
        #endregion
    }
}
