using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace AvaloniaTemplate.Services
{
    public class ScrollBarService : ObservableObject, IScrollBarService
    {
        private const double verticalScrollStep = 25 * 3;
        private const double horizontalScrollStep = 70 * 3;
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

        #region Обновить положение вертикальной прокрутки
        /// <summary>
        /// Обновить положение вертикальной прокрутки
        /// </summary>
        public void UpdateVerticalScrollBarValue(double offset)
        {
            VerticalScrollBarValue = verticalScrollBarOffset - offset * verticalScrollStep;
        }
        #endregion

        #region Обновить положение горизонтальной прокрутки
        /// <summary>
        /// Обновить положение горизонтальной прокрутки
        /// </summary>
        public void UpdateHorizontalScrollBarValue(double offset)
        {
            HorizontalScrollBarValue = horizontalScrollBarOffset - offset * horizontalScrollStep;
        }
        #endregion

        #region Обновить смещение вертикальной прокрутки
        /// <summary>
        /// Обновить смещение вертикальной прокрутки
        /// </summary>
        /// <param name="offset"></param>
        public void UpdateVerticalScrollBarOffset(double offset)
        {
            verticalScrollBarOffset = Math.Clamp(offset, 0, 1000);
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
            horizontalScrollBarOffset = Math.Clamp(offset, 0, 1000);
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
    }
}
