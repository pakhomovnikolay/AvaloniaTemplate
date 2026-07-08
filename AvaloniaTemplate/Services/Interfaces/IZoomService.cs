namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IZoomService
    {
        #region Событие изменения масштаба
        /// <summary>
        /// Событие изменения масштаба
        /// </summary>
        event ScaleChanged ScaleChange;
        delegate void ScaleChanged(double value);
        #endregion

        #region Масштаб
        /// <summary>
        /// Масштаб
        /// </summary>
        double Scale { get; }
        #endregion

        #region Минимальная значение шага изменения мастаба
        /// <summary>
        /// Минимальная значение шага изменения мастаба
        /// </summary>
        double SmallChangeScale { get; set; }
        #endregion

        #region Установить масштаб
        /// <summary>
        /// Установить масштаб
        /// </summary>
        /// <param name="scale"></param>
        void SetScale(double scale);
        #endregion

        #region Пересчитать масштаб
        /// <summary>
        /// Пересчитать масштаб
        /// </summary>
        /// <param name="scale"></param>
        void RecalculateScale(double scale);
        #endregion
    }
}