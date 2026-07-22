namespace AvaloniaTemplate.Models.SourceTable.Base.Interfaces
{
    public interface IModelCellStyle
    {
        #region Цвет заднего фона
        /// <summary>
        /// Цвет заднего фона
        /// </summary>
        string Background { get; set; }
        #endregion

        #region Цвет переднего фона
        /// <summary>
        /// Цвет переднего фона
        /// </summary>
        string Foreground { get; set; }
        #endregion

        #region Цвет границ
        /// <summary>
        /// Цвет границ
        /// </summary>
        string BorderBrush { get; set; }
        #endregion

        #region Шрифт
        /// <summary>
        /// Шрифт
        /// </summary>
        string FontFamily { get; set; }
        #endregion

        #region Размер шрифта
        /// <summary>
        /// Размер шрифта
        /// </summary>
        double FontSize { get; set; }
        #endregion

        #region Полужирный
        /// <summary>
        /// Полужирный
        /// </summary>
        bool IsBold { get; set; }
        #endregion

        #region Курсивный
        /// <summary>
        /// Курсивный
        /// </summary>
        bool IsItalic { get; set; }
        #endregion

        #region Подчеркнутый
        /// <summary>
        /// Подчеркнутый
        /// </summary>
        bool IsUnderline { get; set; }
        #endregion

        #region Стиль нижний границы
        /// <summary>
        /// Стиль нижний границы
        /// </summary>
        string BorderBottomStyle { get; set; }
        #endregion

        #region Стиль верхней границы
        /// <summary>
        /// Стиль верхней границы
        /// </summary>
        string BorderTopStyle { get; set; }
        #endregion

        #region Стиль левой границы
        /// <summary>
        /// Стиль левой границы
        /// </summary>
        string BorderLeftStyle { get; set; }
        #endregion

        #region Стиль правой границы
        /// <summary>
        /// Стиль правой границы
        /// </summary>
        string BorderRightStyle { get; set; }
        #endregion

        #region Горизонтальное положение контента
        /// <summary>
        /// Горизонтальное положение контента
        /// </summary>
        string HorizontalContentAlignment { get; set; }
        #endregion

        #region Вертикальное положение контента
        /// <summary>
        /// Вертикальное положение контента
        /// </summary>
        string VerticalContentAlignment { get; set; }
        #endregion

        #region Переносить текст
        /// <summary>
        /// Переносить текст
        /// </summary>
        bool IsWrap { get; set; }
        #endregion
    }
}