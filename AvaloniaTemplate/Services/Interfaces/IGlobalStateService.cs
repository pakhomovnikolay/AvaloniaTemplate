namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IGlobalStateService
    {
        #region Буфер обмена не пуст
        /// <summary>
        /// Буфер обмена не пуст
        /// </summary>
        bool ClipboardIsNotEmpty { get; set; } 
        #endregion

        #region Индекс выбранного из списка размера шрифта
        /// <summary>
        /// Индекс выбранного из списка размера шрифта
        /// </summary>
        int SelectedIndexFontSize { get; set; }
        #endregion

        #region Стиль шрифта полужирный
        /// <summary>
        /// Стиль шрифта полужирный
        /// </summary>
        bool IsFontWeightBold { get; set; }
        #endregion

        #region Стиль шрифта курсивный
        /// <summary>
        /// Стиль шрифта курсивный
        /// </summary>
        bool IsFontStyleItalic { get; set; }
        #endregion

        #region Стиль текста подчеркнутый
        /// <summary>
        /// Стиль текста подчеркнутый
        /// </summary>
        bool IsTextStyleUnderline { get; set; }
        #endregion
    }
}
