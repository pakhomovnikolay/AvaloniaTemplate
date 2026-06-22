using Avalonia.Threading;
using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;

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
        private IBrush currentBackground = Brushes.Transparent;
        public IBrush CurrentBackground
        {
            get => currentBackground;
            set => SetProperty(ref currentBackground, value);
        }
        #endregion

        #region Текущий цвет текста
        private IBrush currentForeground = Brushes.Transparent;
        public IBrush CurrentForeground
        {
            get => currentForeground;
            set => SetProperty(ref currentForeground, value);
        }
        #endregion
    }
}