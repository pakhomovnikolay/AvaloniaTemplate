using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class ColorHelper
    {
        #region Стандартные цвета заливки
        /// <summary>
        /// Стандартные цвета заливки
        /// </summary>
        public static readonly List<Color> ColorsStandard = [
            Colors.DarkRed,
            Colors.Red,
            Colors.Orange,
            Colors.Yellow,
            Colors.LightGreen,
            Colors.Green,
            Colors.LightBlue,
            Colors.MediumBlue,
            Colors.DarkBlue,
            Colors.DarkViolet
            ];
        #endregion

        #region Цвета для формирвоания схеме цветов
        /// <summary>
        /// Цвета для формирвоания схеме цветов
        /// </summary>
        public static readonly List<Color> ColorsSchema = [
            Colors.Black,
            Colors.White,
            Colors.Red,
            Colors.Orange,
            Colors.Yellow,
            Colors.Green,
            Colors.Turquoise,
            Colors.Blue,
            Colors.Purple,
            Colors.Gray
            ];
        #endregion

        public static readonly double[] Shades = [
            0.80,
            0.60,
            0.40,
            0.20,
            0.00,
            -0.15,
            -0.30,
            -0.45,
            -0.60,
            -0.75
            ];

        public readonly struct HslColor(double h, double s, double l)
        {
            public double H { get; } = h;
            public double S { get; } = s;
            public double L { get; } = l;
        }

        /// <summary>
        /// RGB -> HSL
        /// </summary>
        public static HslColor ColorToHsl(Color color)
        {
            double r = color.R / 255.0;
            double g = color.G / 255.0;
            double b = color.B / 255.0;

            double max = Math.Max(r, Math.Max(g, b));
            double min = Math.Min(r, Math.Min(g, b));

            double h = 0;
            double s;
            double l = (max + min) / 2.0;

            if (Math.Abs(max - min) < 0.0001)
            {
                s = 0;
            }
            else
            {
                double d = max - min;

                s = l > 0.5
                    ? d / (2.0 - max - min)
                    : d / (max + min);

                if (max == r)
                {
                    h = (g - b) / d + (g < b ? 6 : 0);
                }
                else if (max == g)
                {
                    h = (b - r) / d + 2;
                }
                else
                {
                    h = (r - g) / d + 4;
                }

                h /= 6;
            }

            return new HslColor(h, s, l);
        }

        /// <summary>
        /// HSL -> RGB
        /// </summary>
        public static Color HslToColor(double h, double s, double l)
        {
            double r;
            double g;
            double b;

            if (s == 0)
            {
                r = g = b = l;
            }
            else
            {
                double q = l < 0.5
                    ? l * (1 + s)
                    : l + s - l * s;

                double p = 2 * l - q;

                r = HueToRgb(p, q, h + 1.0 / 3.0);
                g = HueToRgb(p, q, h);
                b = HueToRgb(p, q, h - 1.0 / 3.0);
            }

            return Color.FromRgb(
                (byte)Math.Round(r * 255),
                (byte)Math.Round(g * 255),
                (byte)Math.Round(b * 255));
        }

        private static double HueToRgb(double p, double q, double t)
        {
            if (t < 0)
                t += 1;

            if (t > 1)
                t -= 1;

            if (t < 1.0 / 6.0)
                return p + (q - p) * 6 * t;

            if (t < 1.0 / 2.0)
                return q;

            if (t < 2.0 / 3.0)
                return p + (q - p) * (2.0 / 3.0 - t) * 6;

            return p;
        }

        /// <summary>
        /// Изменить яркость цвета.
        /// adjustment:
        ///  0.0  = без изменений
        ///  1.0  = белый
        /// -1.0  = черный
        /// </summary>
        public static Color ChangeLightness(Color color, double adjustment)
        {
            var hsl = ColorToHsl(color);

            double lightness = adjustment >= 0
                ? hsl.L + (1.0 - hsl.L) * adjustment
                : hsl.L * (1.0 + adjustment);

            lightness = Math.Clamp(lightness, 0.0, 1.0);

            return HslToColor(hsl.H, hsl.S, lightness);
        }

        #region Создать цветовую схему
        /// <summary>
        /// Создать цветовую палитру
        /// </summary>
        /// <param name="stateService"></param>
        /// <param name="frame"></param>
        /// <param name="panel"></param>
        /// <param name="panelRecent"></param>
        public static void CreateColorPalet(ICommand command, Popup frame, StackPanel panel)
        {
            panel.Children.Add(Helper.CreateLabel("Схема цветов"));
            panel.Children.Add(CreateColorsSchema(command, frame));
            panel.Children.Add(Helper.CreateLabel("Стандартные цвета"));
            panel.Children.Add(CreateColorsStandard(command, frame));
        }
        #endregion

        #region Создать кнопки выбора цветов
        /// <summary>
        /// Создать кнопки выбора цветов
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Button CreateButtonColor(ICommand command, Popup frame, Color color)
        {
            var button = new Button()
            {
                Height = 20,
                Width = 20,
                Background = new SolidColorBrush(color),
                BorderThickness = new(1),
                BorderBrush = Brushes.Gray,
                CornerRadius = new(1),
                Command = command,
                CommandParameter = new SolidColorBrush(color)
            };

            button.Click += (_, _)
                => frame.Close();

            return button;
        }
        #endregion

        #region Создать схему цветов
        /// <summary>
        /// Создать схему цветов
        /// </summary>
        /// <returns></returns>
        private static StackPanel CreateColorsSchema(ICommand command, Popup frame)
        {
            var stackPanel = Helper.CreateStackPanel();
            foreach (var color in ColorsSchema)
            {
                var button = CreateButtonColor(command, frame, color);
                button.Margin = new(0, 0, 0, 10);

                var childStackPanel = Helper.CreateStackPanel(Orientation.Vertical, 3);
                childStackPanel.Children.Add(button);

                foreach (var shade in Shades)
                {
                    var colorShade = ChangeLightness(color, shade);
                    childStackPanel.Children.Add(CreateButtonColor(command, frame, colorShade));
                }
                stackPanel.Children.Add(childStackPanel);
            }
            return stackPanel;
        }
        #endregion

        #region Создать стандартные цвета
        /// <summary>
        /// Создать стандартные цвета
        /// </summary>
        /// <param name="stateService"></param>
        /// <param name="frame"></param>
        /// <param name="panelRecent"></param>
        /// <returns></returns>
        private static StackPanel CreateColorsStandard(ICommand command, Popup frame)
        {
            var stackPanel = Helper.CreateStackPanel();
            foreach (var color in ColorsStandard)
                stackPanel.Children.Add(CreateButtonColor(command, frame, color));

            return stackPanel;
        }
        #endregion
    }
}
