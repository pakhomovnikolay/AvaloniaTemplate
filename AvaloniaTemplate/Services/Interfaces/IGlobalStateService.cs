namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IGlobalStateService
    {
        #region Индекс выбранного из списка размера шрифта
        /// <summary>
        /// Индекс выбранного из списка размера шрифта
        /// </summary>
        int SelectedIndexFontSize { get; set; } 
        #endregion
    }
}
