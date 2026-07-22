namespace AvaloniaTemplate.Models.SourceTable.Base.Interfaces
{
    public interface IModelBase<T> : ISpreadsheetElement
    {
        #region Идентификатор
        /// <summary>
        /// Идентификатор
        /// </summary>
        string Id { get; set; }
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