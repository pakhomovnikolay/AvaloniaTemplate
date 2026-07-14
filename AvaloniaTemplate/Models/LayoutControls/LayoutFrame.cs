using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Models.LayoutControls.Models;
using System.Collections.Generic;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutFrame : Control
    {
        public List<LayoutModelFrameLine>? RowsY { get; set; } = [];
        public List<LayoutModelFrameLine>? ColsX { get; set; } = [];

        public override void Render(DrawingContext context)
        {
            RenderFrame(context);
        }

        #region Отрисовать сетку
        /// <summary>
        /// Отрисовать сетку
        /// </summary>
        /// <param name="context"></param>
        private void RenderFrame(DrawingContext context)
        {
            if (ColsX is null && RowsY is null)
                return;

            // Вертикальные линии
            if (ColsX.Count > 0)
            {
                foreach (var colX in ColsX)
                {
                    //context.DrawLine(colX.LinePen,
                    //    new Point(colX.Right, colX.PositionY),
                    //    new Point(colX.Right, colX.PositionY + colX.Size)
                    //    );

                    context.DrawLine(colX.LinePen,
                        new Point(colX.PositionX, colX.PositionY),
                        new Point(colX.PositionX, colX.PositionY + colX.Size)
                        );
                }



            }

            // Горизонтальные линии
            if (RowsY.Count > 0)
            {
                foreach (var rowY in RowsY)
                {
                    //context.DrawLine(rowY.LinePen,
                    //    new Point(rowY.PositionX, rowY.Bottom),
                    //    new Point(rowY.PositionX + rowY.Size, rowY.Bottom)
                    //);

                    context.DrawLine(rowY.LinePen,
                        new Point(rowY.PositionX, rowY.PositionY),
                        new Point(rowY.PositionX + rowY.Size, rowY.PositionY)
                        );
                }

            }
        }
        #endregion
    }
}
