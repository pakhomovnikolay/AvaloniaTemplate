using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Models.LayoutControls.Models;
using System.Collections.Generic;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutActiveArea : Control
    {
        public List<LayoutModelActiveArea?>? SelectedAreas { get; set; } = [];
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
            if (SelectedAreas is null || SelectedAreas.Count <= 0)
                return;

            foreach (var SelectedArea in SelectedAreas)
            {
                var rect = SelectedArea.Area ?? new();
                context.DrawRectangle(SelectedArea.RectFill, SelectedArea.RectPen, rect);
            }
        }
        #endregion
    }
}
