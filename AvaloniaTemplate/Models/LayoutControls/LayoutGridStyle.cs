using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Models.Enums;
using System.Collections.Generic;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutGridStyle : Control
    {
        private Dictionary<(string desc, BorderLineStyleType type), (Rect rect, IBrush brush, Pen pen)> dctionaryBorderStyle;

        #region Стиль границ
        /// <summary>
        /// Стиль границ
        /// </summary>
        public BorderStyleType BorderStyle { get; set; } = new()
        {
            Bottom = BorderLineStyleType.None,
            Top = BorderLineStyleType.None,
            Left = BorderLineStyleType.None,
            Right = BorderLineStyleType.None,
            InsideHorizontal = BorderLineStyleType.None,
            InsideVertical = BorderLineStyleType.None
        };
        #endregion

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            if (double.IsNaN(Width) || double.IsNaN(Height) || Width <= 0 || Height <= 0)
                return;

            CreateDictionary();
            if (dctionaryBorderStyle.TryGetValue(("Bottom", BorderStyle.Bottom), out var style))
                RenderGeometry(context, style.rect, style.brush, style.pen);
            if (dctionaryBorderStyle.TryGetValue(("Top", BorderStyle.Top), out style))
                RenderGeometry(context, style.rect, style.brush, style.pen);
            if (dctionaryBorderStyle.TryGetValue(("Left", BorderStyle.Left), out style))
                RenderGeometry(context, style.rect, style.brush, style.pen);
            if (dctionaryBorderStyle.TryGetValue(("Right", BorderStyle.Right), out style))
                RenderGeometry(context, style.rect, style.brush, style.pen);
            if (dctionaryBorderStyle.TryGetValue(("InsideHorizontal", BorderStyle.InsideHorizontal), out style))
                RenderGeometry(context, style.rect, style.brush, style.pen);
            if (dctionaryBorderStyle.TryGetValue(("InsideVertical", BorderStyle.InsideVertical), out style))
                RenderGeometry(context, style.rect, style.brush, style.pen);
        }

        private static void RenderGeometry(DrawingContext context, Rect rect, IBrush brush, Pen pen)
            => context.DrawRectangle(brush, pen, rect);

        private void CreateDictionary()
        {
            var sizeNone = 0.5;
            var sizeNormal = 0.5;
            var sizeThick = 1;
            var thickness = 1.0;

            dctionaryBorderStyle = new()
            {
                [("Bottom", BorderLineStyleType.None)] = (new Rect(0, Height, Width, sizeNone), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
                [("Bottom", BorderLineStyleType.Normal)] = (new Rect(0, Height, Width, sizeNormal), Brushes.Black, new Pen(Brushes.Black, thickness)),
                [("Bottom", BorderLineStyleType.Thick)] = (new Rect(0, Height, Width, sizeThick), Brushes.Black, new Pen(Brushes.Black, thickness)),

                [("Top", BorderLineStyleType.None)] = (new Rect(0, 0, Width, sizeNone), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
                [("Top", BorderLineStyleType.Normal)] = (new Rect(0, 0, Width, sizeNormal), Brushes.Black, new Pen(Brushes.Black, thickness)),
                [("Top", BorderLineStyleType.Thick)] = (new Rect(0, 0, Width, sizeThick), Brushes.Black, new Pen(Brushes.Black, thickness)),

                [("Left", BorderLineStyleType.None)] = (new Rect(0, 0, sizeNone, Height), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
                [("Left", BorderLineStyleType.Normal)] = (new Rect(0, 0, sizeNormal, Height), Brushes.Black, new Pen(Brushes.Black, thickness)),
                [("Left", BorderLineStyleType.Thick)] = (new Rect(0, 0, sizeThick, Height), Brushes.Black, new Pen(Brushes.Black, thickness)),

                [("Right", BorderLineStyleType.None)] = (new Rect(Width, 0, sizeNone, Height), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
                [("Right", BorderLineStyleType.Normal)] = (new Rect(Width, 0, sizeNormal, Height), Brushes.Black, new Pen(Brushes.Black, thickness)),
                [("Right", BorderLineStyleType.Thick)] = (new Rect(Width, 0, sizeThick, Height), Brushes.Black, new Pen(Brushes.Black, thickness)),

                [("InsideHorizontal", BorderLineStyleType.None)] = (new Rect(0, Height / 2, Width, sizeNone), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
                [("InsideHorizontal", BorderLineStyleType.Normal)] = (new Rect(0, Height / 2, Width, sizeNormal), Brushes.Black, new Pen(Brushes.Black)),
                [("InsideHorizontal", BorderLineStyleType.Thick)] = (new Rect(0, Height / 2, Width, sizeThick), Brushes.Black, new Pen(Brushes.Black, thickness)),

                [("InsideVertical", BorderLineStyleType.None)] = (new Rect(Width / 2, 0, sizeNone, Height), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
                [("InsideVertical", BorderLineStyleType.Normal)] = (new Rect(Width / 2, 0, sizeNormal, Height), Brushes.Black, new Pen(Brushes.Black)),
                [("InsideVertical", BorderLineStyleType.Thick)] = (new Rect(Width / 2, 0, sizeThick, Height), Brushes.Black, new Pen(Brushes.Black, thickness))
            };
        }
    }
}
