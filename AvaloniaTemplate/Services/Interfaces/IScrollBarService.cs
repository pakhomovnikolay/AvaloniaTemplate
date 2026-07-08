namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IScrollBarService
    {
        #region Текущее положение вертикальной прокрутки
        /// <summary>
        /// Текущее положение вертикальной прокрутки
        /// </summary>
        double VerticalScrollBarValue { get; set; }
        #endregion

        #region Текущий размер области видимости вертикальной прокрутки
        /// <summary>
        /// Текущий размер области видимости вертикальной прокрутки
        /// </summary>
        double VerticalScrollViewportSize { get; set; }
        #endregion

        #region Максимальный размер контента вертикальной прокрутки
        /// <summary>
        /// Максимальный размер контента вертикальной прокрутки
        /// </summary>
        double VerticalScrollBarMaximum { get; set; }
        #endregion

        #region Текущее положение горизонтальной прокрутки
        /// <summary>
        /// Текущее положение горизонтальной прокрутки
        /// </summary>
        double HorizontalScrollBarValue { get; set; }
        #endregion

        #region Текущий размер области видимости горизонтальной прокрутки
        /// <summary>
        /// Текущий размер области видимости горизонтальной прокрутки
        /// </summary>
        double HorizontalScrollViewportSize { get; set; }
        #endregion

        #region Максимальный размер контента горизонтальной прокрутки
        /// <summary>
        /// Максимальный размер контента горизонтальной прокрутки
        /// </summary>
        double HorizontalScrollBarMaximum { get; set; }
        #endregion

        #region Обновить положение вертикальной прокрутки
        /// <summary>
        /// Обновить положение вертикальной прокрутки
        /// </summary>
        /// <param name="offset"></param>
        void UpdateVerticalScrollBarValue(double offset);
        #endregion

        #region Обновить положение горизонтальной прокрутки
        /// <summary>
        /// Обновить положение горизонтальной прокрутки
        /// </summary>
        /// <param name="offset"></param>
        void UpdateHorizontalScrollBarValue(double offset);
        #endregion

        #region Обновить смещение вертикальной прокрутки
        /// <summary>
        /// Обновить смещение вертикальной прокрутки
        /// </summary>
        /// <param name="offset"></param>
        void UpdateVerticalScrollBarOffset(double offset);
        #endregion

        #region Обновить смещение горизонтальной прокрутки
        /// <summary>
        /// Обновить смещение горизонтальной прокрутки
        /// </summary>
        /// <param name="offset"></param>
        void UpdateHorizontalScrollBarOffset(double offset);
        #endregion

        #region Обновить область просмотра
        /// <summary>
        /// Обновить область просмотра
        /// </summary>
        /// <param name="viewportWidth"></param>
        /// <param name="viewportHeight"></param>
        /// <param name="contentWidth"></param>
        /// <param name="contentHeight"></param>
        void UpdateViewport(double viewportWidth, double viewportHeight, double contentWidth, double contentHeight);
        #endregion

        #region Событие изменения положения полосы прокрутки
        /// <summary>
        /// Событие изменения положения полосы прокрутки
        /// </summary>
        event PositionChanged? PositionChange;
        delegate void PositionChanged(double X, double Y);
        #endregion
    }
}