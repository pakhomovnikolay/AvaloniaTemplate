using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Services.Interfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace AvaloniaTemplate.Services
{
    public class UIConnectorService : ObservableObject, IUIConnectorService
    {
        #region Команда - вставить
        private ICommand command_Paste;
        /// <summary>
        /// Команда - вставить
        /// </summary>
        public ICommand Command_Paste
        {
            get => command_Paste ??= new RelayCommand(ExecuteCommand_Paste);
            set => SetProperty(ref command_Paste, value);
        }

        private void ExecuteCommand_Paste()
        {
            Debug.WriteLine("ExecuteCommand_Paste");
        }
        #endregion

        #region Команда - вырезать
        private ICommand command_Cut;
        /// <summary>
        /// Команда - вырезать
        /// </summary>
        public ICommand Command_Cut
        {
            get => command_Cut ??= new RelayCommand(ExecuteCommand_Cut);
            set => SetProperty(ref command_Cut, value);
        }

        private void ExecuteCommand_Cut()
        {
            Debug.WriteLine("ExecuteCommand_Cut");
        }
        #endregion

        #region Команда - копировать
        private ICommand command_Copy;
        /// <summary>
        /// Команда - копировать
        /// </summary>
        public ICommand Command_Copy
        {
            get => command_Copy ??= new RelayCommand(ExecuteCommand_Copy);
            set => SetProperty(ref command_Copy, value);
        }

        private void ExecuteCommand_Copy()
        {
            Debug.WriteLine("ExecuteCommand_Copy");
        }
        #endregion

        #region Команда - формат по образцу
        private ICommand command_AsSimple;
        /// <summary>
        /// Команда - формат по образцу
        /// </summary>
        public ICommand Command_AsSimple
        {
            get => command_AsSimple ??= new RelayCommand(ExecuteCommand_AsSimple);
            set => SetProperty(ref command_AsSimple, value);
        }

        private void ExecuteCommand_AsSimple()
        {
            Debug.WriteLine("ExecuteCommand_AsSimple");
        }
        #endregion

        #region Команда - установить\снять полужирный стиль текста
        private ICommand command_ChangeFontWeightBold;
        /// <summary>
        /// Команда - установить\снять полужирный стиль текста
        /// </summary>
        public ICommand Command_ChangeFontWeightBold
        {
            get => command_ChangeFontWeightBold ??= new RelayCommand(ExecuteCommand_ChangeFontWeightBold);
            set => SetProperty(ref command_ChangeFontWeightBold, value);
        }

        private void ExecuteCommand_ChangeFontWeightBold()
        {
            Debug.WriteLine("ExecuteCommand_ChangeFontWeightBold");
        }
        #endregion

        #region Команда - установить\снять курсивный стиль текста
        private ICommand command_ChangeFontStyleItalic;
        /// <summary>
        /// Команда - установить\снять курсивный стиль текста
        /// </summary>
        public ICommand Command_ChangeFontStyleItalic
        {
            get => command_ChangeFontStyleItalic ??= new RelayCommand(ExecuteCommand_ChangeFontStyleItalic);
            set => SetProperty(ref command_ChangeFontStyleItalic, value);
        }

        private void ExecuteCommand_ChangeFontStyleItalic()
        {
            Debug.WriteLine("ExecuteCommand_ChangeFontStyleItalic");
        }
        #endregion

        #region Команда - установить\снять стиль текста подчеркнутый
        private ICommand command_ChangeTextUnderline;
        /// <summary>
        /// Команда - установить\снять стиль текста подчеркнутый
        /// </summary>
        public ICommand Command_ChangeTextUnderline
        {
            get => command_ChangeTextUnderline ??= new RelayCommand(ExecuteCommand_ChangeTextUnderline);
            set => SetProperty(ref command_ChangeTextUnderline, value);
        }

        private void ExecuteCommand_ChangeTextUnderline()
        {
            Debug.WriteLine("ExecuteCommand_ChangeTextUnderline");
        }
        #endregion

        #region Команда - установить выбраннй стиль границ
        private ICommand command_SetSelectedStyleBorder;
        /// <summary>
        /// Команда - установить выбраннй стиль границ
        /// </summary>
        public ICommand Command_SetSelectedStyleBorder
        {
            get => command_SetSelectedStyleBorder ??= new RelayCommand(ExecuteCommand_SetSelectedStyleBorder);
            set => SetProperty(ref command_SetSelectedStyleBorder, value);
        }

        private void ExecuteCommand_SetSelectedStyleBorder(object p)
        {
            if (p is null || p is not CurrentBorderStyleType styleType)
                return;

            BorderStyleType = styleType;
            Debug.WriteLine($"{styleType}");
        }
        #endregion

        #region Команда - установить цвет заднего фона
        private ICommand command_SetBackground;
        /// <summary>
        /// Команда - установить цвет заднего фона
        /// </summary>
        public ICommand Command_SetBackground
        {
            get => command_SetBackground ??= new RelayCommand(ExecuteCommand_SetBackground);
            set => SetProperty(ref command_SetBackground, value);
        }

        private void ExecuteCommand_SetBackground(object p)
        {
            if (p is null || p is not IBrush brush)
                return;

            var color = Color.Parse(Helper.GetColor(brush));
            if (!BackgroundColors.Contains(color))
            {
                BackgroundColors.Insert(0, color);
                if (BackgroundColors.Count > 10)
                    BackgroundColors.RemoveAt(BackgroundColors.Count - 1);
            }

            CurrentBackground = brush;
            Debug.WriteLine($"{color}");
        }
        #endregion

        #region Команда - установить цвет переднего фона
        private ICommand сommand_SetForeground;
        /// <summary>
        /// Команда - установить цвет переднего фона
        /// </summary>
        public ICommand Command_SetForeground
        {
            get => сommand_SetForeground ??= new RelayCommand(ExecuteCommand_SetForeground);
            set => SetProperty(ref сommand_SetForeground, value);
        }

        private void ExecuteCommand_SetForeground(object p)
        {
            if (p is null || p is not IBrush brush)
                return;

            var color = Color.Parse(Helper.GetColor(brush));
            if (!ForegroundColors.Contains(color))
            {
                ForegroundColors.Insert(0, color);
                if (ForegroundColors.Count > 10)
                    ForegroundColors.RemoveAt(ForegroundColors.Count - 1);
            }

            CurrentForeground = brush;
            Debug.WriteLine($"{color}");
        }
        #endregion

        #region Команда - сменить положение текста по горинтали
        private ICommand command_SetHorizontalContentAlignment;
        /// <summary>
        /// Команда - сменить положение текста по горинтали
        /// </summary>
        public ICommand Command_SetHorizontalContentAlignment
        {
            get => command_SetHorizontalContentAlignment ??= new RelayCommand(ExecuteCommand_SetHorizontalContentAlignment);
            set => SetProperty(ref command_SetHorizontalContentAlignment, value);
        }

        private void ExecuteCommand_SetHorizontalContentAlignment(object p)
        {
            if (p is null || p is not HorizontalAlignment alignment)
                return;

            HorizontalContentAlignment = alignment;
        }
        #endregion

        #region Команда - сменить положение текста по горинтали
        private ICommand command_SetVerticalContentAlignment;
        /// <summary>
        /// Команда - сменить положение текста по горинтали
        /// </summary>
        public ICommand Command_SetVerticalContentAlignment
        {
            get => command_SetVerticalContentAlignment ??= new RelayCommand(ExecuteCommand_SetVerticalContentAlignment);
            set => SetProperty(ref command_SetVerticalContentAlignment, value);
        }

        private void ExecuteCommand_SetVerticalContentAlignment(object p)
        {
            if (p is null || p is not VerticalAlignment alignment)
                return;

            VerticalContentAlignment = alignment;
        }
        #endregion

        #region Команда - создать элемент для коллекции HorizontalTabStrip
        private ICommand command_CreateItemHorizontalTabStrip;
        /// <summary>
        /// Команда - создать элемент для коллекции HorizontalTabStrip
        /// </summary>
        public ICommand Command_CreateItemHorizontalTabStrip
        {
            get => command_CreateItemHorizontalTabStrip ??= new RelayCommand(ExecuteCommand_CreateItemHorizontalTabStrip);
            set => SetProperty(ref command_CreateItemHorizontalTabStrip, value);
        }

        private void ExecuteCommand_CreateItemHorizontalTabStrip()
        {
            CollectionHorizontalTabStrip.Add($"{CollectionHorizontalTabStrip.Count}");
            SelectedItemHorizontalTabStrip = CollectionHorizontalTabStrip[^1];
        }
        #endregion

        #region Установлен полужирный стиль шрифта
        private bool isFontWeightBold;
        /// <summary>
        /// Установлен полужирный стиль шрифта
        /// </summary>
        public bool IsFontWeightBold
        {
            get => isFontWeightBold;
            set => SetProperty(ref isFontWeightBold, value);
        }
        #endregion

        #region Установлен курсивный стиль шрифта
        private bool isFontStyleItalic;
        /// <summary>
        /// Установлен курсивный стиль шрифта
        /// </summary>
        public bool IsFontStyleItalic
        {
            get => isFontStyleItalic;
            set => SetProperty(ref isFontStyleItalic, value);
        }
        #endregion

        #region Установлен подчеркнутый стиль текста
        private bool isTextUnderline;
        /// <summary>
        /// Установлен подчеркнутый стиль текста
        /// </summary>
        public bool IsTextUnderline
        {
            get => isTextUnderline;
            set => SetProperty(ref isTextUnderline, value);
        }
        #endregion

        #region Текущий тип стиля сетки
        private CurrentBorderStyleType borderStyleType = CurrentBorderStyleType.Bottom;
        /// <summary>
        /// Текущий тип стиля сетки
        /// </summary>
        public CurrentBorderStyleType BorderStyleType
        {
            get => borderStyleType;
            set => SetProperty(ref borderStyleType, value);
        }
        #endregion

        #region Текущий цвет заднего фона
        private IBrush currentBackground = Brushes.Yellow;
        /// <summary>
        /// Текущий цвет заднего фона
        /// </summary>
        public IBrush CurrentBackground
        {
            get => currentBackground;
            set => SetProperty(ref currentBackground, value);
        }
        #endregion

        #region Текущий цвет переднего фона
        private IBrush currentForeground = Brushes.Red;
        /// <summary>
        /// Текущий цвет переднего фона
        /// </summary>
        public IBrush CurrentForeground
        {
            get => currentForeground;
            set => SetProperty(ref currentForeground, value);
        }
        #endregion

        #region Коллекция последних выбранных цветов заднего фона
        private ObservableCollection<Color> backgroundColors = [];
        /// <summary>
        /// Коллекция последних выбранных цветов заднего фона
        /// </summary>
        public ObservableCollection<Color> BackgroundColors
        {
            get => backgroundColors;
            set => SetProperty(ref backgroundColors, value);
        }
        #endregion

        #region Коллекция последних выбранных цветов переднего фона
        private ObservableCollection<Color> foregroundColors = [];
        /// <summary>
        /// Коллекция последних выбранных цветов переднего фона
        /// </summary>
        public ObservableCollection<Color> ForegroundColors
        {
            get => foregroundColors;
            set => SetProperty(ref foregroundColors, value);
        }
        #endregion

        #region Фиксированное состояние кнопки "Формат по образцу"
        private bool isAsSimpleFixed = false;
        /// <summary>
        /// Фиксированное состояние кнопки "Формат по образцу"
        /// </summary>
        public bool IsAsSimpleFixed
        {
            get => isAsSimpleFixed;
            set => SetProperty(ref isAsSimpleFixed, value);
        }
        #endregion

        #region Текущий шрифт
        private FontFamily сurrentFont;
        /// <summary>
        /// Текущий шрифт
        /// </summary>
        public FontFamily CurrentFont
        {
            get => сurrentFont;
            set => SetProperty(ref сurrentFont, value);
        }
        #endregion

        #region Текущий размер шрифт
        private double currentFontSize;
        /// <summary>
        /// Текущий размер шрифт
        /// </summary>
        public double CurrentFontSize
        {
            get => currentFontSize;
            set => SetProperty(ref currentFontSize, value);
        }
        #endregion

        #region Положение текста по горизонтали
        private HorizontalAlignment horizontalContentAlignment = HorizontalAlignment.Stretch;
        /// <summary>
        /// Положение текста по горизонтали
        /// </summary>
        public HorizontalAlignment HorizontalContentAlignment
        {
            get => horizontalContentAlignment;
            set => SetProperty(ref horizontalContentAlignment, value);
        }
        #endregion

        #region Положение текста по вертикали
        private VerticalAlignment verticalContentAlignment = VerticalAlignment.Stretch;
        /// <summary>
        /// Положение текста по вертикали
        /// </summary>
        public VerticalAlignment VerticalContentAlignment
        {
            get => verticalContentAlignment;
            set => SetProperty(ref verticalContentAlignment, value);
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

        #region Коллекция элементов HorizontalTabStrip
        private ObservableCollection<object> collectionHorizontalTabStrip;
        /// <summary>
        /// Коллекция элементов HorizontalTabStrip
        /// </summary>
        public ObservableCollection<object> CollectionHorizontalTabStrip
        {
            get => collectionHorizontalTabStrip;
            set => SetProperty(ref collectionHorizontalTabStrip, value);
        }
        #endregion

        #region Выбранный элемент коллекции HorizontalTabStrip
        private object selectedItemHorizontalTabStrip;
        /// <summary>
        /// Выбранный элемент коллекции HorizontalTabStrip
        /// </summary>
        public object SelectedItemHorizontalTabStrip
        {
            get => selectedItemHorizontalTabStrip;
            set => SetProperty(ref selectedItemHorizontalTabStrip, value);
        }
        #endregion

        #region Текущий масштаб
        private double zoon;
        /// <summary>
        /// Текущий масштаб
        /// </summary>
        public double Zoon
        {
            get => zoon;
            set => SetProperty(ref zoon, value);
        }
        #endregion

        //    private readonly IClipboardService сlipboardService;
        //    private readonly DispatcherTimer timerControlClipboard = new() { Interval = TimeSpan.FromMilliseconds(1000) };

        //    public UIConnectorService()
        //    {
        //        сlipboardService = App.GetService<IClipboardService>();
        //        timerControlClipboard.Start();
        //        timerControlClipboard.Tick += (_, _) =>
        //        {
        //            ClipboardIsNotEmpty = сlipboardService.DataOnClipboardAsync().Result;
        //        };
        //    }

        //    #region Буфер обмена не пуст
        //    private bool сlipboardIsNotEmpty;
        //    public bool ClipboardIsNotEmpty
        //    {
        //        get => сlipboardIsNotEmpty;
        //        set => SetProperty(ref сlipboardIsNotEmpty, value);
        //    }
        //    #endregion

        //    #region Индекс выбранного из списка размера шрифта
        //    private int selectedIndexFontSize;
        //    public int SelectedIndexFontSize
        //    {
        //        get => selectedIndexFontSize;
        //        set => SetProperty(ref selectedIndexFontSize, value);
        //    }
        //    #endregion

        //    #region Стиль шрифта полужирный
        //    private bool fontWeightBold;
        //    public bool IsFontWeightBold
        //    {
        //        get => fontWeightBold;
        //        set => SetProperty(ref fontWeightBold, value);
        //    }
        //    #endregion

        //    #region Стиль шрифта курсивный
        //    private bool fontStyleItalic;
        //    public bool IsFontStyleItalic
        //    {
        //        get => fontStyleItalic;
        //        set => SetProperty(ref fontStyleItalic, value);
        //    }
        //    #endregion

        //    #region Стиль текста подчеркнутый
        //    private bool textStyleUnderline;
        //    public bool IsTextStyleUnderline
        //    {
        //        get => textStyleUnderline;
        //        set => SetProperty(ref textStyleUnderline, value);
        //    }
        //    #endregion

        //    #region Коллекция последних выбранных цветов задний заливки
        //    private ObservableCollection<Color> backgroundColors = [];
        //    public ObservableCollection<Color> BackgroundColors
        //    {
        //        get => backgroundColors;
        //        set => SetProperty(ref backgroundColors, value);
        //    }
        //    #endregion

        //    #region Коллекция последних выбранных цветов передней заливки
        //    private ObservableCollection<Color> foregroundColors = [];
        //    public ObservableCollection<Color> ForegroundColors
        //    {
        //        get => foregroundColors;
        //        set => SetProperty(ref foregroundColors, value);
        //    }
        //    #endregion

        //    #region Текущий цвет заливки
        //    private IBrush currentBackground = Brushes.Yellow;
        //    public IBrush CurrentBackground
        //    {
        //        get => currentBackground;
        //        set => SetProperty(ref currentBackground, value);
        //    }
        //    #endregion

        //    #region Текущий цвет текста
        //    private IBrush currentForeground = Brushes.Red;
        //    public IBrush CurrentForeground
        //    {
        //        get => currentForeground;
        //        set => SetProperty(ref currentForeground, value);
        //    }
        //    #endregion

        //    #region Текущий тип стиля сетки
        //    private CurrentBorderStyleType borderStyleType = CurrentBorderStyleType.Bottom;
        //    public CurrentBorderStyleType BorderStyleType
        //    {
        //        get
        //        {
        //            CurrentBorderStyle ??= GridStyleHelper.CreateGridStyle(borderStyleType, 18, 18);
        //            return borderStyleType;
        //        }
        //        set
        //        {
        //            if (SetProperty(ref borderStyleType, value))
        //                CurrentBorderStyle = GridStyleHelper.CreateGridStyle(borderStyleType, 18, 18);
        //        }
        //    }
        //    #endregion

        //    #region Текущий тип стиля сетки
        //    private Border currentBorderStyle;
        //    public Border CurrentBorderStyle
        //    {
        //        get => currentBorderStyle;
        //        set => SetProperty(ref currentBorderStyle, value);
        //    }
        //    #endregion

        //    #region Текущий стиль шрфита
        //    private FontFamily currentFontFamily;
        //    /// <summary>
        //    /// Текущий стиль шрфита
        //    /// </summary>
        //    public FontFamily CurrentFontFamily
        //    {
        //        get => currentFontFamily;
        //        set => SetProperty(ref currentFontFamily, value);
        //    }
        //    #endregion

        //    #region Текущий размер шрфита
        //    private double currentFontSize;
        //    /// <summary>
        //    /// Текущий размер шрфита
        //    /// </summary>
        //    public double CurrentFontSize
        //    {
        //        get => currentFontSize;
        //        set => SetProperty(ref currentFontSize, value);
        //    }
        //    #endregion

        //    #region Положение текста по горизонтали
        //    private HorizontalAlignment horizontalTextAlignment = HorizontalAlignment.Stretch;
        //    /// <summary>
        //    /// Положение текста по горизонтали
        //    /// </summary>
        //    public HorizontalAlignment HorizontalTextAlignment
        //    {
        //        get => horizontalTextAlignment;
        //        set => SetProperty(ref horizontalTextAlignment, value);
        //    }
        //    #endregion

        //    #region Положение текста по вертикали
        //    private VerticalAlignment verticalTextAlignment = VerticalAlignment.Stretch;
        //    /// <summary>
        //    /// Положение текста по вертикали
        //    /// </summary>
        //    public VerticalAlignment VerticalTextAlignment
        //    {
        //        get => verticalTextAlignment;
        //        set => SetProperty(ref verticalTextAlignment, value);
        //    }
        //    #endregion

        //    #region Перенос текста устанволен
        //    private bool isWrapText;
        //    /// <summary>
        //    /// Перенос текста устанволен
        //    /// </summary>
        //    public bool IsWrapText
        //    {
        //        get => isWrapText;
        //        set => SetProperty(ref isWrapText, value);
        //    }
        //    #endregion

        //    #region Ячейка объеденённая
        //    private bool isMergeCell;
        //    /// <summary>
        //    /// Ячейка объеденённая
        //    /// </summary>
        //    public bool IsMergeCell
        //    {
        //        get => isMergeCell;
        //        set => SetProperty(ref isMergeCell, value);
        //    }
        //    #endregion

        //    #region Текущий режим приложения
        //    private AppActiveModeType appActiveMode = AppActiveModeType.Unknown;
        //    /// <summary>
        //    /// Текущий режим приложения
        //    /// </summary>
        //    public AppActiveModeType AppActiveMode
        //    {
        //        get => appActiveMode;
        //        set => SetProperty(ref appActiveMode, value);
        //    }
        //    #endregion

        //    #region Текущий масштаб
        //    private double zoon = 1;
        //    /// <summary>
        //    /// Текущий масштаб
        //    /// </summary>
        //    public double Zoon
        //    {
        //        get => zoon;
        //        set => SetProperty(ref zoon, value);
        //    }
        //    #endregion

        //    #region Минимальная значение шага изменения мастаба
        //    private double smallChangeSlider = 0.5;
        //    /// <summary>
        //    /// Минимальная значение шага изменения мастаба
        //    /// </summary>
        //    public  double SmallChangeSlider
        //    {
        //        get => smallChangeSlider;
        //        set => SetProperty(ref smallChangeSlider, value);
        //    }
        //    #endregion

        //    #region Максимальная значение шага изменения мастаба
        //    private double largeChangeSlider = 1.5;
        //    /// <summary>
        //    /// Максимальная значение шага изменения мастаба
        //    /// </summary>
        //    public double LargeChangeSlider
        //    {
        //        get => largeChangeSlider;
        //        set => SetProperty(ref largeChangeSlider, value);
        //    }
        //    #endregion

        //    #region Запрос на обновление масштаба

        //    /// <summary>
        //    /// Запрос на обновление масштаба
        //    /// </summary>
        //    /// <param name="delta"></param>
        //    public void UpdateZoomRequested(double delta)
        //    {
        //        Zoon += delta * (SmallChangeSlider / 10);
        //    }
        //    #endregion
        //}
    }
}