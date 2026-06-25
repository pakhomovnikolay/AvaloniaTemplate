using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;

namespace AvaloniaTemplate.Services
{
    public class GlobalStateService : ObservableObject, IGlobalStateService
    {
        private readonly IClipboardService сlipboardService;
        private readonly DispatcherTimer timerControlClipboard = new() { Interval = TimeSpan.FromMilliseconds(1000) };

        public GlobalStateService()
        {
            сlipboardService = App.GetService<IClipboardService>();
            timerControlClipboard.Start();
            timerControlClipboard.Tick += (_, _) =>
            {
                ClipboardIsNotEmpty = сlipboardService.DataOnClipboardAsync().Result;
            };
        }

        #region Буфер обмена не пуст
        private bool сlipboardIsNotEmpty;
        public bool ClipboardIsNotEmpty
        {
            get => сlipboardIsNotEmpty;
            set => SetProperty(ref сlipboardIsNotEmpty, value);
        }
        #endregion

        #region Индекс выбранного из списка размера шрифта
        private int selectedIndexFontSize;
        public int SelectedIndexFontSize
        {
            get => selectedIndexFontSize;
            set => SetProperty(ref selectedIndexFontSize, value);
        }
        #endregion

        #region Стиль шрифта полужирный
        private bool fontWeightBold;
        public bool IsFontWeightBold
        {
            get => fontWeightBold;
            set => SetProperty(ref fontWeightBold, value);
        }
        #endregion

        #region Стиль шрифта курсивный
        private bool fontStyleItalic;
        public bool IsFontStyleItalic
        {
            get => fontStyleItalic;
            set => SetProperty(ref fontStyleItalic, value);
        }
        #endregion

        #region Стиль текста подчеркнутый
        private bool textStyleUnderline;
        public bool IsTextStyleUnderline
        {
            get => textStyleUnderline;
            set => SetProperty(ref textStyleUnderline, value);
        }
        #endregion

        #region Коллекция последних выбранных цветов задний заливки
        private ObservableCollection<Color> backgroundColors = [];
        public ObservableCollection<Color> BackgroundColors
        {
            get => backgroundColors;
            set => SetProperty(ref backgroundColors, value);
        }
        #endregion

        #region Коллекция последних выбранных цветов передней заливки
        private ObservableCollection<Color> foregroundColors = [];
        public ObservableCollection<Color> ForegroundColors
        {
            get => foregroundColors;
            set => SetProperty(ref foregroundColors, value);
        }
        #endregion

        #region Текущий цвет заливки
        private IBrush currentBackground = Brushes.Yellow;
        public IBrush CurrentBackground
        {
            get => currentBackground;
            set => SetProperty(ref currentBackground, value);
        }
        #endregion

        #region Текущий цвет текста
        private IBrush currentForeground = Brushes.Red;
        public IBrush CurrentForeground
        {
            get => currentForeground;
            set => SetProperty(ref currentForeground, value);
        }
        #endregion

        #region Текущий тип стиля сетки
        private CurrentBorderStyleType borderStyleType = CurrentBorderStyleType.Bottom;
        public CurrentBorderStyleType BorderStyleType
        {
            get
            {
                CurrentBorderStyle ??= GridStyleHelper.CreateGridStyle(borderStyleType, 18, 18);
                return borderStyleType;
            }
            set
            {
                if (SetProperty(ref borderStyleType, value))
                    CurrentBorderStyle = GridStyleHelper.CreateGridStyle(borderStyleType, 18, 18);
            }
        }
        #endregion

        #region Текущий тип стиля сетки
        private Border currentBorderStyle;
        public Border CurrentBorderStyle
        {
            get => currentBorderStyle;
            set => SetProperty(ref currentBorderStyle, value);
        }
        #endregion

        #region Текущий стиль шрфита
        private FontFamily currentFontFamily;
        /// <summary>
        /// Текущий стиль шрфита
        /// </summary>
        public FontFamily CurrentFontFamily
        {
            get => currentFontFamily;
            set => SetProperty(ref currentFontFamily, value);
        }
        #endregion

        #region Текущий размер шрфита
        private double currentFontSize;
        /// <summary>
        /// Текущий размер шрфита
        /// </summary>
        public double CurrentFontSize
        {
            get => currentFontSize;
            set => SetProperty(ref currentFontSize, value);
        }
        #endregion

        #region Положение текста по горизонтали
        private HorizontalAlignment horizontalTextAlignment = HorizontalAlignment.Stretch;
        /// <summary>
        /// Положение текста по горизонтали
        /// </summary>
        public HorizontalAlignment HorizontalTextAlignment
        {
            get => horizontalTextAlignment;
            set => SetProperty(ref horizontalTextAlignment, value);
        }
        #endregion

        #region Положение текста по вертикали
        private VerticalAlignment verticalTextAlignment = VerticalAlignment.Stretch;
        /// <summary>
        /// Положение текста по вертикали
        /// </summary>
        public VerticalAlignment VerticalTextAlignment
        {
            get => verticalTextAlignment;
            set => SetProperty(ref verticalTextAlignment, value);
        }
        #endregion

        #region Перенос текста устанволен
        private bool isWrapText;
        /// <summary>
        /// Перенос текста устанволен
        /// </summary>
        public bool IsWrapText
        {
            get => isWrapText;
            set => SetProperty(ref isWrapText, value);
        }
        #endregion

        #region Ячейка объеденённая
        private bool isMergeCell;
        /// <summary>
        /// Ячейка объеденённая
        /// </summary>
        public bool IsMergeCell
        {
            get => isMergeCell;
            set => SetProperty(ref isMergeCell, value);
        }
        #endregion

        #region Текущий режим приложения
        private AppActiveModeType appActiveMode = AppActiveModeType.Unknown;
        /// <summary>
        /// Текущий режим приложения
        /// </summary>
        public AppActiveModeType AppActiveMode
        {
            get => appActiveMode;
            set => SetProperty(ref appActiveMode, value);
        }
        #endregion
    }
}