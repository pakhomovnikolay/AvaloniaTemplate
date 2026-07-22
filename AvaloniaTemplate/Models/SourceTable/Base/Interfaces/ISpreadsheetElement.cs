namespace AvaloniaTemplate.Models.SourceTable.Base.Interfaces
{
    public interface ISpreadsheetElement
    {
        #region Геометрия позиционирования
        /// <summary>
        /// Геометрия позиционирования
        /// </summary>
        Geometry Geometry { get; init; } 
        #endregion
    }
}