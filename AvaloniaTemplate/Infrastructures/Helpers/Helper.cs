using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Media.TextFormatting;
using Avalonia.Platform;
using AvaloniaTemplate.Infrastructures.Constants;
using System;

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
            var resource = App.Desktop?.MainWindow?.FindResource(name);
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
            => color.ToString();
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
    }
}
