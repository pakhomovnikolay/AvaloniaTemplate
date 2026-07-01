using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Models;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary;
using SkiaSharp;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IUIConnectorService
    {
        #region Команда - вставить
        /// <summary>
        /// Команда - вставить
        /// </summary>
        ICommand Command_Paste { get; set; }
        #endregion

        #region Команда - вырезать
        /// <summary>
        /// Команда - вырезать
        /// </summary>
        ICommand Command_Cut { get; set; }
        #endregion

        #region Команда - копировать
        /// <summary>
        /// Команда - копировать
        /// </summary>
        ICommand Command_Copy { get; set; }
        #endregion

        #region Команда - формат по образцу
        /// <summary>
        /// Команда - формат по образцу
        /// </summary>
        ICommand Command_AsSimple { get; set; }
        #endregion

        #region Команда - установить\снять полужирный стиль текста
        /// <summary>
        /// Команда - установить\снять полужирный стиль текста
        /// </summary>
        ICommand Command_ChangeFontWeightBold { get; set; }
        #endregion

        #region Команда - установить\снять курсивный стиль текста
        /// <summary>
        /// Команда - установить\снять курсивный стиль текста
        /// </summary>
        ICommand Command_ChangeFontStyleItalic { get; set; }
        #endregion

        #region Команда - установить\снять стиль текста подчеркнутый
        /// <summary>
        /// Команда - установить\снять стиль текста подчеркнутый
        /// </summary>
        ICommand Command_ChangeTextUnderline { get; set; }
        #endregion

        #region Установлен полужирный стиль шрифта
        /// <summary>
        /// Установлен полужирный стиль шрифта
        /// </summary>
        bool IsFontWeightBold { get; set; }
        #endregion

        #region Установлен курсивный стиль шрифта
        /// <summary>
        /// Установлен курсивный стиль шрифта
        /// </summary>
        bool IsFontStyleItalic { get; set; }
        #endregion

        #region Установлен подчеркнутый стиль текста
        /// <summary>
        /// Установлен подчеркнутый стиль текста
        /// </summary>
        bool IsTextUnderline { get; set; } 
        #endregion



        //InitializeFontStyle("Ж", FontWeight.Bold) },
        //    { TemplateButtonFontStyleToolkitType.FontStyleItalic, () => InitializeFontStyle("К", fontStyle: FontStyle.Italic) },
        //    { TemplateButtonFontStyleToolkitType.TextStyleUnderLine, () => InitializeFontStyle("Ч", IsUnderline: true) },

        //#region Буфер обмена не пуст
        ///// <summary>
        ///// Буфер обмена не пуст
        ///// </summary>
        //bool ClipboardIsNotEmpty { get; set; } 
        //#endregion

        //#region Индекс выбранного из списка размера шрифта
        ///// <summary>
        ///// Индекс выбранного из списка размера шрифта
        ///// </summary>
        //int SelectedIndexFontSize { get; set; }
        //#endregion

        //#region Стиль шрифта полужирный
        ///// <summary>
        ///// Стиль шрифта полужирный
        ///// </summary>
        //bool IsFontWeightBold { get; set; }
        //#endregion

        //#region Стиль шрифта курсивный
        ///// <summary>
        ///// Стиль шрифта курсивный
        ///// </summary>
        //bool IsFontStyleItalic { get; set; }
        //#endregion

        //#region Стиль текста подчеркнутый
        ///// <summary>
        ///// Стиль текста подчеркнутый
        ///// </summary>
        //bool IsTextStyleUnderline { get; set; }
        //#endregion

        //#region Коллекция последних выбранных цветов задний заливки
        ///// <summary>
        ///// Коллекция последних выбранных цветов задний заливки
        ///// </summary>
        //ObservableCollection<Color> BackgroundColors { get; set; }
        //#endregion

        //#region Коллекция последних выбранных цветов передней заливки
        ///// <summary>
        ///// Коллекция последних выбранных цветов передней заливки
        ///// </summary>
        //ObservableCollection<Color> ForegroundColors { get; set; }
        //#endregion

        //#region Текущий цвет заливки
        ///// <summary>
        ///// Текущий цвет заливки
        ///// </summary>
        //IBrush CurrentBackground { get; set; }
        //#endregion

        //#region Текущий цвет текста
        ///// <summary>
        ///// Текущий цвет текста
        ///// </summary>
        //IBrush CurrentForeground { get; set; }
        //#endregion

        //#region Текущий тип стиля сетки
        ///// <summary>
        ///// Текущий тип стиля сетки
        ///// </summary>
        //CurrentBorderStyleType BorderStyleType { get; set; }
        //#endregion

        //#region Текущий стиль сетки
        ///// <summary>
        ///// Текущий стиль сетки
        ///// </summary>
        //Border CurrentBorderStyle { get; set; }
        //#endregion

        //#region Текущий стиль шрфита
        ///// <summary>
        ///// Текущий стиль шрфита
        ///// </summary>
        //FontFamily CurrentFontFamily { get; set; }
        //#endregion

        //#region Текущий размер шрфита
        ///// <summary>
        ///// Текущий размер шрфита
        ///// </summary>
        //double CurrentFontSize { get; set; }
        //#endregion

        //#region Положение текста по горизонтали
        ///// <summary>
        ///// Положение текста по горизонтали
        ///// </summary>
        //HorizontalAlignment HorizontalTextAlignment { get; set; }
        //#endregion

        //#region Положение текста по вертикали
        ///// <summary>
        ///// Положение текста по вертикали
        ///// </summary>
        //VerticalAlignment VerticalTextAlignment { get; set; }
        //#endregion

        //#region Перенос текста устанволен
        ///// <summary>
        ///// Перенос текста устанволен
        ///// </summary>
        //bool IsWrapText { get; set; }
        //#endregion

        //#region Ячейка объеденённая
        ///// <summary>
        ///// Ячейка объеденённая
        ///// </summary>
        //bool IsMergeCell { get; set; }
        //#endregion

        //#region Текущий режим приложения
        ///// <summary>
        ///// Текущий режим приложения
        ///// </summary>
        //AppActiveModeType AppActiveMode { get; set; }
        //#endregion

        //#region Текущий масштаб
        ///// <summary>
        ///// Текущий масштаб
        ///// </summary>
        //double Zoon { get; set; }
        //#endregion

        //#region Минимальная значение шага изменения мастаба
        ///// <summary>
        ///// Минимальная значение шага изменения мастаба
        ///// </summary>
        //double SmallChangeSlider { get; set; }
        //#endregion

        //#region Максимальная значение шага изменения мастаба
        ///// <summary>
        ///// Максимальная значение шага изменения мастаба
        ///// </summary>
        //double LargeChangeSlider { get; set; }
        //#endregion

        //#region Запрос на обновление масштаба
        ///// <summary>
        ///// Запрос на обновление масштаба
        ///// </summary>
        ///// <param name="delta"></param>
        //void UpdateZoomRequested(double delta); 
        //#endregion
    }
}