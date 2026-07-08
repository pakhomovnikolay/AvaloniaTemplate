namespace AvaloniaTemplate.Models.Table.Base.Interfaces
{
    public interface IModelBase<T>
    {
        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        string Id { get; set; }
        #endregion

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

        #region Видимость
        /// <summary>
        /// Видимость
        /// </summary>
        bool IsVisible { get; set; }
        #endregion

        #region Выбрана
        /// <summary>
        /// Выбрана
        /// </summary>
        bool IsSelected { get; set; }
        #endregion

        #region Активная
        /// <summary>
        /// Активная
        /// </summary>
        bool IsFocused { get; set; }
        #endregion

        #region Стиль ячейки
        /// <summary>
        /// Стиль ячейки
        /// </summary>
        ModelCellStyle CellStyle { get; set; }
        #endregion

        #region Корневой элемент
        /// <summary>
        /// Корневой элемент
        /// </summary>
        T Owner { get; set; } 
        #endregion
    }
}