using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.LayoutControls;
using System.Collections.Generic;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class GridStyleHelper
    {
        private const double width = 16;
        private const double height = 16;

        #region Словарь одиночных линий сетки
        /// <summary>
        /// Словарь одиночных линий сетки
        /// </summary>
        public static readonly Dictionary<CurrentBorderStyleType, string> SingleBorderStyleTypes = new()
        {
            { CurrentBorderStyleType.Bottom, "Нижняя граница" },
            { CurrentBorderStyleType.Top, "Верхняя граница" },
            { CurrentBorderStyleType.Left, "Левая граница" },
            { CurrentBorderStyleType.Right, "Правая граница" }
        };
        #endregion

        #region Словарь линий сетки
        /// <summary>
        /// Словарь линий сетки
        /// </summary>
        public static readonly Dictionary<CurrentBorderStyleType, string> BorderStyleTypes = new()
        {
            { CurrentBorderStyleType.None, "Нет границ" },
            { CurrentBorderStyleType.All, "Все границы" },
            { CurrentBorderStyleType.Outside, "Внешние границы" },
            { CurrentBorderStyleType.ThickOutside, "Толстые внешние границы" }
        };
        #endregion

        #region Дополнительный словарь линий сетки
        /// <summary>
        /// Дополнительный словарь линий сетки
        /// </summary>
        public static readonly Dictionary<CurrentBorderStyleType, string> AdditionBorderStyleTypes = new()
        {
            { CurrentBorderStyleType.DoubleBottom, "Двойнвя нижняя граница" },
            { CurrentBorderStyleType.ThickBottom, "Толстая нижняя граница" },
            { CurrentBorderStyleType.TopBottom, "Верхняя и нижняя границы" },
        };
        #endregion

        #region Создать стили сетки
        /// <summary>
        /// Создать стили сетки
        /// </summary>
        public static void CreateGridStyle(ICommand command, StackPanel stackPanel, Popup frame)
        {
            foreach (var style in SingleBorderStyleTypes)
                stackPanel.Children.Add(CreateButtonGrid(style.Key, style.Value, frame, command));

            stackPanel.Children.Add(new Separator());

            foreach (var style in BorderStyleTypes)
                stackPanel.Children.Add(CreateButtonGrid(style.Key, style.Value, frame, command));
        }
        #endregion

        #region Создать визуальное представление стиля сетки
        /// <summary>
        /// Создать визуальное представление стиля сетки
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="borderStyle"></param>
        /// <returns></returns>
        public static Border CreateGridStyle(CurrentBorderStyleType borderStyle, double width = width, double height = height)
        {
            var border = new Border()
            {
                Padding = new(0),
                Background = Brushes.White,
                Height = height,
                Width = width,
                Child = borderStyle switch
                {
                    CurrentBorderStyleType.Bottom => CreateGrid(width, height, Bottom: BorderLineStyleType.Normal),
                    CurrentBorderStyleType.Top => CreateGrid(width, height, Top: BorderLineStyleType.Normal),
                    CurrentBorderStyleType.Left => CreateGrid(width, height, Left: BorderLineStyleType.Normal),
                    CurrentBorderStyleType.Right => CreateGrid(width, height, Right: BorderLineStyleType.Normal),
                    CurrentBorderStyleType.All => CreateGrid(width, height,
                        BorderLineStyleType.Normal,
                        BorderLineStyleType.Normal,
                        BorderLineStyleType.Normal,
                        BorderLineStyleType.Normal,
                        BorderLineStyleType.Normal,
                        BorderLineStyleType.Normal),

                    CurrentBorderStyleType.Outside => CreateGrid(width, height,
                        BorderLineStyleType.Normal,
                        BorderLineStyleType.Normal,
                        BorderLineStyleType.Normal,
                        BorderLineStyleType.Normal
                        ),

                    CurrentBorderStyleType.ThickOutside => CreateGrid(width - 1, height - 1,
                        BorderLineStyleType.Thick,
                        BorderLineStyleType.Thick,
                        BorderLineStyleType.Thick,
                        BorderLineStyleType.Thick
                        ),

                    _ => CreateGrid(width, height)
                }
            };

            return border;
        } 
        #endregion

        private static Button CreateButtonGrid(CurrentBorderStyleType borderStyleType, string borderStyleDesc, Popup frame, ICommand? command = null)
        {
            var label = Helper.CreateLabel(borderStyleDesc);
            label.Margin = new(10, 0, 0, 0);
            label.VerticalAlignment = VerticalAlignment.Center;

            var grid = new Grid()
            {
                ColumnDefinitions = [
                    new ColumnDefinition(25, GridUnitType.Pixel),
                    new ColumnDefinition(25, GridUnitType.Star)
                    ]
            };
            var border = CreateGridStyle(borderStyleType);
            border.MinHeight = 22;
            border.MinWidth = 22;

            Grid.SetColumn(label, 1);
            Grid.SetColumn(border, 0);
            grid.Children.Add(label);
            grid.Children.Add(border);

            var button = new Button()
            {
                Content = grid,
                Padding = new(5),
                BorderThickness = new(0),
                CornerRadius = new(0),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                VerticalContentAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Background = Brushes.Transparent,
                Command = command,
                CommandParameter = borderStyleType
            };
            button.Classes.Add("highlightedBackground");
            button.Click += (_, _)
                => frame?.Close();
            return button;
        }

        private static LayoutGridStyle CreateGrid(
            double width,
            double height,
            BorderLineStyleType Bottom = BorderLineStyleType.None,
            BorderLineStyleType Top = BorderLineStyleType.None,
            BorderLineStyleType Left = BorderLineStyleType.None,
            BorderLineStyleType Right = BorderLineStyleType.None,
            BorderLineStyleType InsideHorizontal = BorderLineStyleType.None,
            BorderLineStyleType InsideVertical = BorderLineStyleType.None)
        {

            var control = new LayoutGridStyle()
            {
                Width = width,
                Height = height,
                BorderStyle = new()
                {
                    Bottom = Bottom,
                    Top = Top,
                    Left = Left,
                    Right = Right,
                    InsideHorizontal = InsideHorizontal,
                    InsideVertical = InsideVertical,
                }
            };
            control.InvalidateVisual();
            return control;
        }
    }
}
