using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Models.Enums;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutGridStyle : Control
    {
        /// <summary>
        /// Стиль верхней границы
        /// </summary>
        public BorderStyleType Bottom { get; set; } = BorderStyleType.None;

        /// <summary>
        /// Стиль нижней границы
        /// </summary>
        public BorderStyleType Top { get; set; } = BorderStyleType.None;

        /// <summary>
        /// Стиль левой границы
        /// </summary>
        public BorderStyleType Left { get; set; } = BorderStyleType.None;

        /// <summary>
        /// Стиль правой границы
        /// </summary>
        public BorderStyleType Right { get; set; } = BorderStyleType.None;

        /// <summary>
        /// Стиль внутренней границы по горизонтали
        /// </summary>
        public BorderStyleType InsideHorizontal { get; set; } = BorderStyleType.None;

        /// <summary>
        /// Стиль внутренней границы по вертикали
        /// </summary>
        public BorderStyleType InsideVertical { get; set; } = BorderStyleType.None;


        public override void Render(DrawingContext context)
        {
            base.Render(context);

            var widthBottomTop = Width;
            var widthLeftRightNormal = 0.5;
            var widthLeftRightThick = 1.3;

            var heightLeftRight = Height;
            var heightBottomTopNormal = 0.5;
            var heightBottomTopThick = 1.3;

            var fillNormal = Brushes.Transparent;
            var fillThick = Brushes.Black;

            var penNone = new Pen(Brushes.Gray, 1, dashStyle: DashStyle.Dash);
            var pen = new Pen(Brushes.Black, 1);

            if (Bottom == BorderStyleType.None)
            {
                var rect = new Rect(0, Height, Width, heightBottomTopNormal);
                context.DrawRectangle(fillNormal, penNone, rect);
            }
            else
            {
                if (Bottom == BorderStyleType.Normal)
                {
                    var rect = new Rect(0, Height, Width, heightBottomTopNormal);
                    context.DrawRectangle(fillNormal, pen, rect);
                }
                else if (Bottom == BorderStyleType.Thick)
                {
                    var rect = new Rect(0, Height, Width, heightBottomTopThick);
                    context.DrawRectangle(fillThick, pen, rect);
                }
            }

            if (Top == BorderStyleType.None)
            {
                var rect = new Rect(0, 0, Width, heightBottomTopNormal);
                context.DrawRectangle(fillNormal, penNone, rect);
            }
            else
            {
                if (Top == BorderStyleType.Normal)
                {
                    var rect = new Rect(0, 0, Width, heightBottomTopNormal);
                    context.DrawRectangle(fillNormal, pen, rect);
                }
                else if (Top == BorderStyleType.Thick)
                {
                    var rect = new Rect(0, 0, Width, heightBottomTopThick);
                    context.DrawRectangle(fillThick, pen, rect);
                }
            }

            if (Left == BorderStyleType.None)
            {
                var rect = new Rect(0, 0, widthLeftRightNormal, Height);
                context.DrawRectangle(fillNormal, penNone, rect);
            }
            else
            {
                if (Left == BorderStyleType.Normal)
                {
                    var rect = new Rect(0, 0, widthLeftRightNormal, Height);
                    context.DrawRectangle(fillNormal, pen, rect);
                }
                else if (Left == BorderStyleType.Thick)
                {
                    var rect = new Rect(0, 0, widthLeftRightThick, Height);
                    context.DrawRectangle(fillThick, pen, rect);
                }
            }

            if (Right == BorderStyleType.None)
            {
                var rect = new Rect(Width, 0, widthLeftRightNormal, Height);
                context.DrawRectangle(fillNormal, penNone, rect);
            }
            else
            {
                if (Right == BorderStyleType.Normal)
                {
                    var rect = new Rect(Width, 0, widthLeftRightNormal, Height);
                    context.DrawRectangle(fillNormal, pen, rect);
                }
                else if (Right == BorderStyleType.Thick)
                {
                    var rect = new Rect(Width, 0, widthLeftRightThick, Height + 1);
                    context.DrawRectangle(fillThick, pen, rect);
                }
            }

            if (InsideHorizontal == BorderStyleType.None)
            {
                var rect = new Rect(0, Height / 2, Width, heightBottomTopNormal);
                context.DrawRectangle(fillNormal, penNone, rect);
            }
            else
            {
                if (InsideHorizontal == BorderStyleType.Normal)
                {
                    var rect = new Rect(0, Height / 2, Width, heightBottomTopNormal);
                    context.DrawRectangle(fillNormal, pen, rect);
                }
                else if (InsideHorizontal == BorderStyleType.Thick)
                {
                    var rect = new Rect(0, Height / 2, Width, heightBottomTopThick);
                    context.DrawRectangle(fillThick, pen, rect);
                }
            }

            if (InsideVertical == BorderStyleType.None)
            {
                var rect = new Rect(Width / 2, 0, widthLeftRightNormal, Height);
                context.DrawRectangle(fillNormal, penNone, rect);
            }
            else
            {
                if (InsideVertical == BorderStyleType.Normal)
                {
                    var rect = new Rect(Width / 2, 0, widthLeftRightNormal, Height);
                    context.DrawRectangle(fillNormal, pen, rect);
                }
                else if (InsideVertical == BorderStyleType.Thick)
                {
                    var rect = new Rect(Width / 2, 0, widthLeftRightThick, Height);
                    context.DrawRectangle(fillThick, pen, rect);
                }
            }
        }
    }
}
