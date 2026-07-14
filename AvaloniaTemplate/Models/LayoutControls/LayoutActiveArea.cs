using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Models.LayoutControls.Models;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutActiveArea : Control
    {
        public LayoutModelActiveArea? SelectedArea { get; set; }
        public override void Render(DrawingContext context)
        {
            RenderSelectedArea(context);
        }

        #region Отрисовать текущую область
        /// <summary>
        /// Отрисовать текущую область
        /// </summary>
        /// <param name="context"></param>
        private void RenderSelectedArea(DrawingContext context)
        {
            if (SelectedArea is null)
                return;

            var rect = SelectedArea.Area ?? new();
            context.DrawRectangle(SelectedArea.RectFill, SelectedArea.RectPen, rect);
        }
        #endregion
    }
}
