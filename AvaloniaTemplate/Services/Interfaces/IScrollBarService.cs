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

        #region Текущее положение горизонтальной прокрутки
        /// <summary>
        /// Текущее положение горизонтальной прокрутки
        /// </summary>
        double HorizontalScrollBarValue { get; set; }
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

        #region Событие изменения положения полосы прокрутки
        /// <summary>
        /// Событие изменения положения полосы прокрутки
        /// </summary>
        event PositionChanged? PositionChange;
        delegate void PositionChanged(double X, double Y);
        #endregion
    }
}
