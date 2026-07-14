namespace AvaloniaTemplate.Models.Table.Base.Interfaces
{
    public interface ISpreadsheetElement
    {
        #region Ширина
        /// <summary>
        /// Ширина
        /// </summary>
        double Width { get; set; }
        #endregion

        #region Высота
        /// <summary>
        /// Высота
        /// </summary>
        double Height { get; set; }
        #endregion

        #region Положение по оси X
        /// <summary>
        /// Положение по оси X
        /// </summary>
        double PositionX { get; set; }
        #endregion

        #region Положение по оси Y
        /// <summary>
        /// Положение по оси Y
        /// </summary>
        double PositionY { get; set; }
        #endregion

        #region Координата крайней точки по оси X
        /// <summary>
        /// Координата крайней точки по оси X
        /// </summary>
        double Right { get; set; }
        #endregion

        #region Координата крайней точки по оси Y
        /// <summary>
        /// Координата крайней точки по оси Y
        /// </summary>
        double Bottom { get; set; }
        #endregion
    }
}