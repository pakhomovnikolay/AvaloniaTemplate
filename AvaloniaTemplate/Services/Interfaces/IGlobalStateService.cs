using Avalonia.Controls;
using Avalonia.Media;
using AvaloniaTemplate.Models.Enums;
using System.Collections.ObjectModel;

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

        #region Коллекция последних выбранных цветов задний заливки
        /// <summary>
        /// Коллекция последних выбранных цветов задний заливки
        /// </summary>
        ObservableCollection<Color> BackgroundColors { get; set; }
        #endregion

        #region Коллекция последних выбранных цветов передней заливки
        /// <summary>
        /// Коллекция последних выбранных цветов передней заливки
        /// </summary>
        ObservableCollection<Color> ForegroundColors { get; set; }
        #endregion

        #region Текущий цвет заливки
        /// <summary>
        /// Текущий цвет заливки
        /// </summary>
        IBrush CurrentBackground { get; set; }
        #endregion

        #region Текущий цвет текста
        /// <summary>
        /// Текущий цвет текста
        /// </summary>
        IBrush CurrentForeground { get; set; }
        #endregion

        #region Текущий тип стиля сетки
        /// <summary>
        /// Текущий тип стиля сетки
        /// </summary>
        CurrentBorderStyleType BorderStyleType { get; set; }
        #endregion

        #region Текущий стиль сетки
        /// <summary>
        /// Текущий стиль сетки
        /// </summary>
        Border CurrentBorderStyle { get; set; }
        #endregion

        #region Текущий стиль шрфита
        /// <summary>
        /// Текущий стиль шрфита
        /// </summary>
        FontFamily CurrentFontFamily { get; set; }
        #endregion

        #region Текущий размер шрфита
        /// <summary>
        /// Текущий размер шрфита
        /// </summary>
        double CurrentFontSize { get; set; }
        #endregion
    }
}
