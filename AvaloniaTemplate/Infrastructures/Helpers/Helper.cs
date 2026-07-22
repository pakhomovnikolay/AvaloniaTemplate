using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using Avalonia.Platform;
using AvaloniaTemplate.Infrastructures.Constants;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class Helper
    {
        #region Получить ресурс
        /// <summary>
        /// Получить ресурс
        /// </summary>
        /// <typeparam name="T"> Тип желаемого ресурса </typeparam>
        /// <param name="name"> Имя желаемого ресурса </param>
        /// <returns> В случае отсутсвия требуемого ресурса или тип найденного ресурса отличается от требуемого, возвращает - default </returns>
        public static T? GetResource<T>(string name)
        {
            var result = default(T);
            //var resource = App.Desktop?.MainWindow?.FindResource(name);
            var resource = Application.Current?.FindResource(name);

            if (resource is not null && resource is T found)
                result = found;

            return result;

        }
        #endregion

        #region Загрузить и получить изображение
        /// <summary>
        /// Загрузить и получить изображение
        /// </summary>
        /// <param name="nameIcon"></param>
        /// <returns></returns>
        public static Bitmap LoadIcon(string nameIcon)
        {
            var uri = new Uri($"avares://{App.AppName}/Assets/{nameIcon}");
            using var stream = AssetLoader.Open(uri);
            return new Bitmap(stream);
        }
        #endregion

        #region Получить цвет из строки
        /// <summary>
        /// Получить цвет из строки
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IBrush GetColor(string color)
            => Brush.Parse(color);
        #endregion

        #region Получить строку из цвета
        /// <summary>
        /// Получить строку из цвета
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string GetColor(IBrush color)
            => color?.ToString();
        #endregion

        #region Наблюдатель
        /// <summary>
        /// Наблюдатель
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="onNext"></param>
        public class Observer<T>(Action<T> onNext) : IObserver<T>
        {
            private readonly Action<T> _onNext = onNext;
            public void OnNext(T value) => _onNext(value);
            public void OnCompleted() { }
            public void OnError(Exception error) { }
        }
        #endregion

        #region Измерить и получить ширины текста
        /// <summary>
        /// Измерить и получить ширины текста
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static double MeasureTextWidth(string text, double fontSize, FontFamily font)
        {
            if (string.IsNullOrEmpty(text))
                return ConstantsMain.__MinWidthText;

            var typeface = new Typeface(font);
            var textLayout = new TextLayout(
                text,
                typeface,
                fontSize,
                Brushes.Black,
                textAlignment: TextAlignment.Left,
                textWrapping: TextWrapping.NoWrap);

            double measuredWidth = textLayout.Width + ConstantsMain.__MinWidthText;
            return measuredWidth;
        }
        #endregion

        #region Измерить и получить высоту текста
        /// <summary>
        /// Измерить и получить высоту текста
        /// </summary>
        /// <param name="text"></param>
        /// <param name="fontSize"></param>
        /// <param name="font"></param>
        /// <returns></returns>
        public static double MeasureTextHeight(string text, double fontSize, FontFamily font)
        {
            if (string.IsNullOrEmpty(text))
                return ConstantsMain.__MinHeightText;

            var typeface = new Typeface(font);
            var textLayout = new TextLayout(
                text,
                typeface,
                fontSize,
                Brushes.Black,
                textAlignment: TextAlignment.Left,
                textWrapping: TextWrapping.NoWrap);

            double measuredWidth = textLayout.Height + ConstantsMain.__MinHeightText;
            return measuredWidth;
        }
        #endregion

        #region Получить возможный для выбора элемент
        /// <summary>
        /// Получить возможный для выбора элемент
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="index"></param>
        /// <param name="collections"></param>
        /// <returns></returns>
        public static T GetSelectedElement<T>(int index, IList collections)
        {
            T selected = default;
            if (collections?.Count > 0)
            {
                if (index > 0 && index >= collections.Count)
                    selected = (T)collections[^1];
                else if (index >= 0)
                    selected = (T)collections[index];
                else
                    selected = (T)collections[0];
            }
            return selected;
        }
        #endregion

        #region Установить бит
        /// <summary>
        /// Установить бит
        /// </summary>
        /// <param name="status"></param>
        /// <param name="state"></param>
        /// <param name="b_num"></param>
        public static ushort SetBit(ushort status, bool state, byte b_num)
        {
            var value = 0;


            if (state)
                value |= (ushort)(1 << b_num);
            else
                value &= (ushort)~(1 << b_num);

            return (ushort)(value | status);
        }
        #endregion

        #region Получить бит
        /// <summary>
        /// Получить бит
        /// </summary>
        /// <param name="state"></param>
        /// <param name="status"></param>
        /// <param name="b_num"></param>
        public static bool GetBit(ushort status, byte b_num)
        {
            return (status & (1 << b_num)) > 0;
        }
        #endregion

        #region Асинхронное чтение данных
        /// <summary>
        /// Асинхронное чтение данных
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="buffer"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        /// <exception cref="EndOfStreamException"></exception>
        public static async Task ReadExactlyAsync(Stream stream, byte[] buffer, CancellationToken ct)
        {
            int offset = 0;
            while (offset < buffer.Length)
            {
                int read = await stream.ReadAsync(buffer.AsMemory(offset), ct);
                if (read == 0)
                    throw new EndOfStreamException();

                offset += read;
            }
        }
        #endregion

        #region Получить цвет подсветки
        /// <summary>
        /// Получить цвет подсветки
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Color GetAutoHighlight(Color baseColor, double factor = 0.15)
        {
            double luminance = 0.299 * baseColor.R +
                               0.587 * baseColor.G +
                               0.114 * baseColor.B;

            // порог можно подстроить
            bool isLight = luminance > 34;

            return isLight
                ? Darken(baseColor, factor)   // светлый → затемняем
                : Lighten(baseColor, factor); // тёмный → осветляем
        }

        /// <summary>
        /// Получить цвет подсветки
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Color GetAutoHighlight(IBrush baseColor, double factor = 0.15)
        {
            return GetAutoHighlight(Color.Parse(baseColor.ToString()), factor);
        }

        /// <summary>
        /// Получить цвет подсветки
        /// </summary>
        /// <param name="baseColor"></param>
        /// <param name="factor"></param>
        /// <returns></returns>
        public static Color GetAutoHighlight(string baseColor, double factor = 0.15)
        {
            return GetAutoHighlight(Color.Parse(baseColor), factor);
        }
        private static Color Lighten(Color color, double factor)
        {
            return Color.FromArgb(
                color.A,
                (byte)(color.R + (255 - color.R) * factor),
                (byte)(color.G + (255 - color.G) * factor),
                (byte)(color.B + (255 - color.B) * factor));
        }
        private static Color Darken(Color color, double factor)
        {
            return Color.FromArgb(
                color.A,
                (byte)(color.R * (1 - factor)),
                (byte)(color.G * (1 - factor)),
                (byte)(color.B * (1 - factor)));
        }
        #endregion



        #region Создать подрись
        /// <summary>
        /// Создать подрись
        /// </summary>
        /// <param name="label"></param>
        /// <returns></returns>
        public static TextBlock CreateLabel(string label)
        {
            return new TextBlock()
            {
                Text = label,
                FontWeight = FontWeight.Bold,
                Margin = new(5, 5, 0, 5),
            };
        }
        #endregion

        #region Создать StackPanel
        /// <summary>
        /// Создать StackPanel
        /// </summary>
        /// <returns></returns>
        public static StackPanel CreateStackPanel(
            Orientation orientation = Orientation.Horizontal,
            double spacing = 3) => new()
            {
                Orientation = orientation,
                Spacing = spacing
            };
        #endregion

        #region Получить границы из строк
        /// <summary>
        /// Получить границы из строк
        /// </summary>
        /// <param name="left"></param>
        /// <param name="top"></param>
        /// <param name="right"></param>
        /// <param name="bottom"></param>
        /// <returns></returns>
        public static Thickness GetThickness(string left, string top, string right, string bottom)
        {
            _ = int.TryParse(left, out var thicknessLeft);
            _ = int.TryParse(top, out var thicknessTop);
            _ = int.TryParse(right, out var thicknessRight);
            _ = int.TryParse(bottom, out var thicknessBottom);

            return new(thicknessLeft, thicknessTop, thicknessRight, thicknessBottom);
        }
        #endregion

        #region Получить шрифт из текста
        /// <summary>
        /// Получить шрифт из текста
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static FontFamily GetFontFamily(string font)
        {
            return string.IsNullOrWhiteSpace(font)
                ? FontFamilyHelper.FontDefault
                : FontFamilyHelper.FontFamilies?.FirstOrDefault(x => x.Name == font) ?? new(font);
        }
        #endregion

        #region Получить расположение по горизонтали из текста
        /// <summary>
        /// Получить расположение по горизонтали из текста
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static HorizontalAlignment GetHorizontalAlignment(string alignment)
        {
            return alignment.ToLower() switch
            {
                "left" => HorizontalAlignment.Left,
                "center" => HorizontalAlignment.Center,
                "right" => HorizontalAlignment.Right,
                _ => HorizontalAlignment.Stretch,
            };
        }
        #endregion

        #region Получить расположение по вертикали из текста
        /// <summary>
        /// Получить расположение по вертикали из текста
        /// </summary>
        /// <param name="font"></param>
        /// <returns></returns>
        public static VerticalAlignment GetVerticalAlignment(string alignment)
        {
            return alignment.ToLower() switch
            {
                "top" => VerticalAlignment.Top,
                "center" => VerticalAlignment.Center,
                "bottom" => VerticalAlignment.Bottom,
                _ => VerticalAlignment.Stretch,
            };
        }
        #endregion
    }
}