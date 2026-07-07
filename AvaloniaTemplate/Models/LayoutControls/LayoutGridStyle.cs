using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Models.Enums;
using System.Collections.Generic;

namespace AvaloniaTemplate.Models.LayoutControls
{
    public class LayoutGridStyle : Control
    {
        private Dictionary<BorderStyleType, List<BorderSegment>> DictionaryBorderStyle { get; }

        public BorderStyleType CurrentBorderStyleType { get; set; }

        public LayoutGridStyle()
        {
            DictionaryBorderStyle = new()
            {
                {
                    BorderStyleType.Bottom,
                    [
                        new () { Type = BorderSegmentType.Bottom, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Top, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Left, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Right, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideHorizontal, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideVertical, Style = BorderLineStyleType.Dash }
                    ]
                },
                {
                    BorderStyleType.Top,
                    [
                        new () { Type = BorderSegmentType.Bottom, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Top, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Left, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Right, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideHorizontal, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideVertical, Style = BorderLineStyleType.Dash }
                    ]
                },
                {
                    BorderStyleType.Left,
                    [
                        new () { Type = BorderSegmentType.Bottom, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Top, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Left, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Right, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideHorizontal, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideVertical, Style = BorderLineStyleType.Dash }
                    ]
                },
                {
                    BorderStyleType.Right,
                    [
                        new () { Type = BorderSegmentType.Bottom, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Top, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Left, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Right, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.InsideHorizontal, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideVertical, Style = BorderLineStyleType.Dash }
                    ]
                },
                {
                    BorderStyleType.None,
                    [
                        new () { Type = BorderSegmentType.Bottom, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Top, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Left, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.Right, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideHorizontal, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideVertical, Style = BorderLineStyleType.Dash }
                    ]
                },
                {
                    BorderStyleType.All,
                    [
                        new () { Type = BorderSegmentType.Bottom, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Top, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Left, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Right, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.InsideHorizontal, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.InsideVertical, Style = BorderLineStyleType.Normal }
                    ]
                },
                {
                    BorderStyleType.Outside,
                    [
                        new () { Type = BorderSegmentType.Bottom, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Top, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Left, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.Right, Style = BorderLineStyleType.Normal },
                        new () { Type = BorderSegmentType.InsideHorizontal, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideVertical, Style = BorderLineStyleType.Dash }
                    ]
                },
                {
                    BorderStyleType.ThickOutside,
                    [
                        new () { Type = BorderSegmentType.Bottom, Style = BorderLineStyleType.Thick },
                        new () { Type = BorderSegmentType.Top, Style = BorderLineStyleType.Thick },
                        new () { Type = BorderSegmentType.Left, Style = BorderLineStyleType.Thick },
                        new () { Type = BorderSegmentType.Right, Style = BorderLineStyleType.Thick },
                        new () { Type = BorderSegmentType.InsideHorizontal, Style = BorderLineStyleType.Dash },
                        new () { Type = BorderSegmentType.InsideVertical, Style = BorderLineStyleType.Dash }
                    ]
                }
            };
        }


        //    #region Словарь типов взаимодействия с буфером обмена
        //    /// <summary>
        //    /// Словарь типов взаимодействия с буфером обмена
        //    /// </summary>
        //    private static readonly Dictionary<TemplatrButtonClipboardType, (Uri imagePath, string header, Orientation orientationType)> dictionaryClipboardType = new()
        //{
        //    { TemplatrButtonClipboardType.Paste, (new Uri("avares://AvaloniaTemplate/Assets/Paste.png"), "Вставить", Orientation.Vertical) },
        //    { TemplatrButtonClipboardType.Copy, (new Uri("avares://AvaloniaTemplate/Assets/Copy.png"), "Копировать", Orientation.Horizontal) },
        //    { TemplatrButtonClipboardType.Cut, (new Uri("avares://AvaloniaTemplate/Assets/Cut.png"), "Вырезать", Orientation.Horizontal) },
        //    { TemplatrButtonClipboardType.AsSimple, (new Uri("avares://AvaloniaTemplate/Assets/AsSimple.png"), "Формат по образцу", Orientation.Horizontal) }
        //};
        //    #endregion



        private static Pen CreatePen(BorderLineStyleType style)
        {
            return style switch
            {
                BorderLineStyleType.Dash => new Pen(Brushes.Gray, 1) { DashStyle = DashStyle.Dash },
                BorderLineStyleType.Normal => new Pen(Brushes.Black, 1),
                BorderLineStyleType.Thick => new Pen(Brushes.Black, 2),
                _ => new Pen(Brushes.Transparent, 0)
            };
        }

        private void DrawSegment(DrawingContext context, BorderSegment segment)
        {
            var pen = CreatePen(segment.Style);
            var w = Bounds.Width;
            var h = Bounds.Height;

            switch (segment.Type)
            {
                case BorderSegmentType.Bottom:
                    context.DrawLine(pen, new Point(0, h - 0.5), new Point(w, h - 0.5));
                    break;

                case BorderSegmentType.Top:
                    context.DrawLine(pen, new Point(0, 0.5), new Point(w, 0.5));
                    break;

                case BorderSegmentType.Left:
                    context.DrawLine(pen, new Point(0.5, 0), new Point(0.5, h));
                    break;

                case BorderSegmentType.Right:
                    context.DrawLine(pen, new Point(w - 0.5, 0), new Point(w - 0.5, h));
                    break;

                case BorderSegmentType.InsideHorizontal:
                    context.DrawLine(pen, new Point(0, h / 2 + 0.25), new Point(w, h / 2 + 0.25));
                    break;

                case BorderSegmentType.InsideVertical:
                    context.DrawLine(pen, new Point((w - 0.5) / 2, 0), new Point((w - 0.5) / 2, h));
                    break;
            }
        }

        public override void Render(DrawingContext context)
        {
            base.Render(context);
            if (double.IsNaN(Width) || double.IsNaN(Height) || Width <= 0 || Height <= 0 || !DictionaryBorderStyle.TryGetValue(CurrentBorderStyleType, out var segments))
                return;

            foreach (var segment in segments)
                DrawSegment(context, segment);
        }






        //private Dictionary<(string desc, BorderLineStyleType type), (Rect rect, IBrush brush, Pen pen)> dctionaryBorderStyle;

        //#region Стиль границ
        ///// <summary>
        ///// Стиль границ
        ///// </summary>
        //public BorderStyleType BorderStyle { get; set; } = new()
        //{
        //    Bottom = BorderLineStyleType.None,
        //    Top = BorderLineStyleType.None,
        //    Left = BorderLineStyleType.None,
        //    Right = BorderLineStyleType.None,
        //    InsideHorizontal = BorderLineStyleType.None,
        //    InsideVertical = BorderLineStyleType.None
        //};
        //#endregion

        //public override void Render(DrawingContext context)
        //{
        //    base.Render(context);
        //    if (double.IsNaN(Width) || double.IsNaN(Height) || Width <= 0 || Height <= 0)
        //        return;

        //    CreateDictionary();
        //    if (dctionaryBorderStyle.TryGetValue(("Bottom", BorderStyle.Bottom), out var style))
        //        RenderGeometry(context, style.rect, style.brush, style.pen);
        //    if (dctionaryBorderStyle.TryGetValue(("Top", BorderStyle.Top), out style))
        //        RenderGeometry(context, style.rect, style.brush, style.pen);
        //    if (dctionaryBorderStyle.TryGetValue(("Left", BorderStyle.Left), out style))
        //        RenderGeometry(context, style.rect, style.brush, style.pen);
        //    if (dctionaryBorderStyle.TryGetValue(("Right", BorderStyle.Right), out style))
        //        RenderGeometry(context, style.rect, style.brush, style.pen);
        //    if (dctionaryBorderStyle.TryGetValue(("InsideHorizontal", BorderStyle.InsideHorizontal), out style))
        //        RenderGeometry(context, style.rect, style.brush, style.pen);
        //    if (dctionaryBorderStyle.TryGetValue(("InsideVertical", BorderStyle.InsideVertical), out style))
        //        RenderGeometry(context, style.rect, style.brush, style.pen);
        //}

        //private static void RenderGeometry(DrawingContext context, Rect rect, IBrush brush, Pen pen)
        //    => context.DrawRectangle(brush, pen, rect);

        //private void CreateDictionary()
        //{
        //    var sizeNone = 1;
        //    var sizeNormal = 1;
        //    var sizeThick = 2;
        //    var thickness = 0;

        //    dctionaryBorderStyle = new()
        //    {
        //        [("Bottom", BorderLineStyleType.None)] = (new Rect(0.5, Height, Width + 1, sizeNone), Brushes.Gray, new Pen(Brushes.Transparent, thickness, dashStyle: DashStyle.Dash)),
        //        [("Bottom", BorderLineStyleType.Normal)] = (new Rect(0.5, Height, Width + 1, sizeNormal), Brushes.Black, new Pen(Brushes.Transparent, thickness)),
        //        [("Bottom", BorderLineStyleType.Thick)] = (new Rect(0.5, Height, Width + 1, sizeThick), Brushes.Black, new Pen(Brushes.Transparent, thickness)),

        //        [("Top", BorderLineStyleType.None)] = (new Rect(0, 0, Width + 1, sizeNone), Brushes.Gray, new Pen(Brushes.Transparent, thickness, dashStyle: DashStyle.Dash)),
        //        [("Top", BorderLineStyleType.Normal)] = (new Rect(0, 0, Width + 1, sizeNormal), Brushes.Black, new Pen(Brushes.Transparent, thickness)),
        //        [("Top", BorderLineStyleType.Thick)] = (new Rect(0, 0, Width + 1, sizeThick), Brushes.Black, new Pen(Brushes.Transparent, thickness)),

        //        [("Left", BorderLineStyleType.None)] = (new Rect(0, 0, sizeNone, Height), Brushes.Gray, new Pen(Brushes.Transparent, thickness, dashStyle: DashStyle.Dash)),
        //        [("Left", BorderLineStyleType.Normal)] = (new Rect(0.5, 0, sizeNormal, Height), Brushes.Black, new Pen(Brushes.Transparent, thickness)),
        //        [("Left", BorderLineStyleType.Thick)] = (new Rect(0.5, 0, sizeThick, Height), Brushes.Black, new Pen(Brushes.Transparent, thickness)),

        //        [("Right", BorderLineStyleType.None)] = (new Rect(Width, 0, sizeNone, Height + 1), Brushes.Gray, new Pen(Brushes.Transparent, thickness, dashStyle: DashStyle.Dash)),
        //        [("Right", BorderLineStyleType.Normal)] = (new Rect(Width, 0, sizeNormal, Height + 1), Brushes.Black, new Pen(Brushes.Transparent, thickness)),
        //        [("Right", BorderLineStyleType.Thick)] = (new Rect(Width, 0, sizeThick, Height + 1.5), Brushes.Black, new Pen(Brushes.Transparent, thickness)),

        //        [("InsideHorizontal", BorderLineStyleType.None)] = (new Rect(0, Height / 2, Width, sizeNone), Brushes.Gray, new Pen(Brushes.Transparent, thickness, dashStyle: DashStyle.Dash)),
        //        [("InsideHorizontal", BorderLineStyleType.Normal)] = (new Rect(0, Height / 2, Width, sizeNone), Brushes.Black, new Pen(Brushes.Transparent, thickness)),
        //        [("InsideHorizontal", BorderLineStyleType.Thick)] = (new Rect(0, Height / 2, Width, sizeNone), Brushes.Black, new Pen(Brushes.Transparent, thickness)),




        //        //[("Bottom", BorderLineStyleType.Normal)] = (new Rect(0, Height, Width + sizeNone, sizeNormal), Brushes.Black, new Pen(Brushes.Black, thickness)),
        //        //[("Bottom", BorderLineStyleType.Thick)] = (new Rect(0, Height, Width + sizeThick, sizeThick), Brushes.Black, new Pen(Brushes.Black, thickness)),

        //        //[("Top", BorderLineStyleType.None)] = (new Rect(0, 0, Width, sizeNone), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
        //        //[("Top", BorderLineStyleType.Normal)] = (new Rect(0, 0, Width, sizeNormal), Brushes.Black, new Pen(Brushes.Black, thickness)),
        //        //[("Top", BorderLineStyleType.Thick)] = (new Rect(0, 0, Width, sizeThick), Brushes.Black, new Pen(Brushes.Black, thickness)),

        //        //[("Left", BorderLineStyleType.None)] = (new Rect(0, 0, sizeNone, Height), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
        //        //[("Left", BorderLineStyleType.Normal)] = (new Rect(0, 0, sizeNormal, Height), Brushes.Black, new Pen(Brushes.Black, thickness)),
        //        //[("Left", BorderLineStyleType.Thick)] = (new Rect(0, 0, sizeThick, Height), Brushes.Black, new Pen(Brushes.Black, thickness)),

        //        //[("Right", BorderLineStyleType.None)] = (new Rect(Width, 0, sizeNone, Height + sizeNone), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
        //        //[("Right", BorderLineStyleType.Normal)] = (new Rect(Width, 0, sizeNormal, Height + sizeNone), Brushes.Black, new Pen(Brushes.Black, thickness)),
        //        //[("Right", BorderLineStyleType.Thick)] = (new Rect(Width, 0, sizeThick, Height + sizeNormal), Brushes.Black, new Pen(Brushes.Black, thickness)),

        //        //[("InsideHorizontal", BorderLineStyleType.None)] = (new Rect(0, Height / 2, Width, sizeNone), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
        //        //[("InsideHorizontal", BorderLineStyleType.Normal)] = (new Rect(0, Height / 2, Width, sizeNormal), Brushes.Black, new Pen(Brushes.Black, thickness)),
        //        //[("InsideHorizontal", BorderLineStyleType.Thick)] = (new Rect(0, Height / 2, Width, sizeThick), Brushes.Black, new Pen(Brushes.Black, thickness)),

        //        //[("InsideVertical", BorderLineStyleType.None)] = (new Rect(Width / 2, 0, sizeNone, Height), Brushes.Gray, new Pen(Brushes.Gray, thickness, dashStyle: DashStyle.Dash)),
        //        //[("InsideVertical", BorderLineStyleType.Normal)] = (new Rect(Width / 2, 0, sizeNormal, Height), Brushes.Black, new Pen(Brushes.Black, thickness)),
        //        //[("InsideVertical", BorderLineStyleType.Thick)] = (new Rect(Width / 2, 0, sizeThick, Height), Brushes.Black, new Pen(Brushes.Black, thickness))
        //    };
        //}
    }
}
