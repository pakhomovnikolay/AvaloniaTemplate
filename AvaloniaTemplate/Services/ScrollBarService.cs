using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AvaloniaTemplate.Services
{
    public class ScrollBarService : ObservableObject, IScrollBarService
    {
        private const double verticalScrollStep = 25 * 3;
        private const double horizontalScrollStep = 70 * 4;
        private double verticalScrollBarOffset;
        private double horizontalScrollBarOffset;

        #region Текущее положение вертикальной прокрутки
        private double verticalScrollBarValue;
        /// <summary>
        /// Текущее положение вертикальной прокрутки
        /// </summary>
        public double VerticalScrollBarValue
        {
            get => verticalScrollBarValue;
            set => SetProperty(ref verticalScrollBarValue, value);
        }
        #endregion

        #region Текущий размер области видимости вертикальной прокрутки
        private double verticalScrollViewportSize;
        /// <summary>
        /// Текущий размер области видимости вертикальной прокрутки
        /// </summary>
        public double VerticalScrollViewportSize
        {
            get => verticalScrollViewportSize;
            set => SetProperty(ref verticalScrollViewportSize, value);
        }
        #endregion

        #region Максимальный размер контента вертикальной прокрутки
        private double verticalScrollBarMaximum;
        /// <summary>
        /// Максимальный размер контента вертикальной прокрутки
        /// </summary>
        public double VerticalScrollBarMaximum
        {
            get => verticalScrollBarMaximum;
            set => SetProperty(ref verticalScrollBarMaximum, value);
        }
        #endregion

        #region Текущее положение горизонтальной прокрутки
        private double horizontalScrollBarValue;
        /// <summary>
        /// Текущее положение горизонтальной прокрутки
        /// </summary>
        public double HorizontalScrollBarValue
        {
            get => horizontalScrollBarValue;
            set => SetProperty(ref horizontalScrollBarValue, value);
        }
        #endregion

        #region Текущий размер области видимости горизонтальной прокрутки
        private double horizontalScrollViewportSize;
        /// <summary>
        /// Текущий размер области видимости горизонтальной прокрутки
        /// </summary>
        public double HorizontalScrollViewportSize
        {
            get => horizontalScrollViewportSize;
            set => SetProperty(ref horizontalScrollViewportSize, value);
        }
        #endregion

        #region Максимальный размер контента горизонтальной прокрутки
        private double horizontalScrollBarMaximum;
        /// <summary>
        /// Максимальный размер контента горизонтальной прокрутки
        /// </summary>
        public double HorizontalScrollBarMaximum
        {
            get => horizontalScrollBarMaximum;
            set => SetProperty(ref horizontalScrollBarMaximum, value);
        }
        #endregion

        #region Обновить положение вертикальной прокрутки
        /// <summary>
        /// Обновить положение вертикальной прокрутки
        /// </summary>
        public void UpdateVerticalScrollBarValue(double offset)
        {
            VerticalScrollBarValue = verticalScrollBarOffset - offset * verticalScrollStep;
            if (VerticalScrollBarValue < 0)
                VerticalScrollBarValue = 0;
        }
        #endregion

        #region Обновить положение горизонтальной прокрутки
        /// <summary>
        /// Обновить положение горизонтальной прокрутки
        /// </summary>
        public void UpdateHorizontalScrollBarValue(double offset)
        {
            HorizontalScrollBarValue = horizontalScrollBarOffset - offset * horizontalScrollStep;
            if (HorizontalScrollBarValue < 0)
                HorizontalScrollBarValue = 0;
        }
        #endregion

        #region Обновить смещение вертикальной прокрутки
        /// <summary>
        /// Обновить смещение вертикальной прокрутки
        /// </summary>
        /// <param name="offset"></param>
        public void UpdateVerticalScrollBarOffset(double offset)
        {
            verticalScrollBarOffset = Math.Clamp(offset, 0, 100000);
            PositionChange?.Invoke(HorizontalScrollBarValue, VerticalScrollBarValue);
        }
        #endregion

        #region Обновить смещение горизонтальной прокрутки
        /// <summary>
        /// Обновить смещение горизонтальной прокрутки
        /// </summary>
        /// <param name="offset"></param>
        public void UpdateHorizontalScrollBarOffset(double offset)
        {
            horizontalScrollBarOffset = Math.Clamp(offset, 0, 100000);
            PositionChange?.Invoke(HorizontalScrollBarValue, VerticalScrollBarValue);
        }
        #endregion

        #region Событие изменения положения полосы прокрутки
        /// <summary>
        /// Событие изменения положения полосы прокрутки
        /// </summary>
        public event IScrollBarService.PositionChanged? PositionChange;
        public delegate void PositionChanged(double X, double Y);
        #endregion

        #region Обновить область просмотра
        /// <summary>
        /// Обновить область просмотра
        /// </summary>
        /// <param name="viewportWidth"></param>
        /// <param name="viewportHeight"></param>
        /// <param name="contentWidth"></param>
        /// <param name="contentHeight"></param>
        public void UpdateViewport(double viewportWidth, double viewportHeight, double contentWidth, double contentHeight)
        {
            VerticalScrollViewportSize = viewportHeight;
            VerticalScrollBarMaximum = Math.Max(0, contentHeight - viewportHeight);
            HorizontalScrollViewportSize = viewportWidth;
            HorizontalScrollBarMaximum = Math.Max(0, contentWidth - viewportWidth);
        }
        #endregion
    }
}