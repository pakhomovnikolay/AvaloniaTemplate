using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Controls;

public class SelectorPicker : BaseTemplatedControl
{
    static SelectorPicker()
    {
        BorderStyleTypeProperty.Changed.AddClassHandler<SelectorPicker>((x, _) => x.RenderGridStyle(x.BorderStyleType));
    }

    public SelectorPicker()
    {
        dictionaryColorPickerType = new()
        {
            { SelectorPickerType.Background, InitializeBackgroundElement },
            { SelectorPickerType.Foreground, InitializeForegroundElement },
            { SelectorPickerType.BorderStyle, InitializeBorderStyleElement }
        };
    }

    #region Словарь типов выбора фона
    /// <summary>
    /// Словарь типов выбора фона
    /// </summary>
    private readonly Dictionary<SelectorPickerType, Action<Popup>> dictionaryColorPickerType = [];
    #endregion

    #region Тип выбора фона
    public static readonly StyledProperty<SelectorPickerType> PickerTypeProperty =
        AvaloniaProperty.Register<SelectorPicker, SelectorPickerType>(nameof(PickerType));

    /// <summary>
    /// Тип выбора фона
    /// </summary>
    public SelectorPickerType PickerType
    {
        get => GetValue(PickerTypeProperty);
        set => SetValue(PickerTypeProperty, value);
    }
    #endregion

    #region Тип границы
    public static readonly StyledProperty<CurrentBorderStyleType> BorderStyleTypeProperty =
        AvaloniaProperty.Register<GridStylePicker, CurrentBorderStyleType>(nameof(BorderStyleType));

    /// <summary>
    /// Тип границы
    /// </summary>
    public CurrentBorderStyleType BorderStyleType
    {
        get => GetValue(BorderStyleTypeProperty);
        set => SetValue(BorderStyleTypeProperty, value);
    }
    #endregion

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<SelectorPicker, HorizontalAlignment>(nameof(HorizontalContentAlignment));

    /// <summary>
    /// Положение контента по горинтали
    /// </summary>
    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }
    #endregion

    #region Положение контента по вертикали
    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
        AvaloniaProperty.Register<SelectorPicker, VerticalAlignment>(nameof(VerticalContentAlignment));

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<SelectorPicker, object?>(nameof(Content), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных
    /// </summary>
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    #endregion

    #region Окно раскрыто
    public static readonly StyledProperty<bool> IsPopupOpenProperty =
        AvaloniaProperty.Register<SelectorPicker, bool>(nameof(IsPopupOpen), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Окно раскрыто
    /// </summary>
    public bool IsPopupOpen
    {
        get => GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }
    #endregion

    #region Источник данных раскрывающегося окна
    public static readonly StyledProperty<Panel?> ContentPopupProperty =
        AvaloniaProperty.Register<SelectorPicker, Panel?>(nameof(ContentPopup), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных раскрывающегося окна
    /// </summary>
    public Panel? ContentPopup
    {
        get => GetValue(ContentPopupProperty);
        set => SetValue(ContentPopupProperty, value);
    }
    #endregion

    #region Источник данных раскрывающегося окна
    public static readonly StyledProperty<ObservableCollection<Color>> BackgroundColorsProperty =
        AvaloniaProperty.Register<SelectorPicker, ObservableCollection<Color>>(nameof(BackgroundColors), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных раскрывающегося окна
    /// </summary>
    public ObservableCollection<Color> BackgroundColors
    {
        get => GetValue(BackgroundColorsProperty);
        set => SetValue(BackgroundColorsProperty, value);
    }
    #endregion

    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<SelectorPicker, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Команда
    /// </summary>
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    #endregion

    #region Параметр для команды
    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<SelectorPicker, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    private void InitializeBackgroundElement(Popup frame = null)
    {
        var stackPanel = Helper.CreateStackPanel(Orientation.Vertical, 5);
        var LayoutColorsRecent = Helper.CreateStackPanel();

        ColorHelper.CreateColorPalet(Command, frame, stackPanel);
        stackPanel.Children.Add(Helper.CreateLabel("Недавние цвета"));
        stackPanel.Children.Add(LayoutColorsRecent);

        stackPanel.Children.Add(new Separator() { Margin = new(0) });
        stackPanel.Children.Add(CreateButtonClearColor(Command, frame));
        stackPanel.Children.Add(new Separator() { Margin = new(0) });

        frame.Opened += (_, _) => CreateColorsRecent(Command, frame, LayoutColorsRecent);
        var gridPanel = CreateGridPanel();
        var image = new Image()
        {
            Width = 18,
            Height = 18,
            Source = new Bitmap(AssetLoader.Open(new Uri("avares://AvaloniaTemplate/Assets/FillingColor.png")))
        };
        Grid.SetRow(image, 0);
        gridPanel.Children.Add(image);

        Content = gridPanel;
        ContentPopup = new()
        {
            Width = 230,
            Children = { stackPanel }
        };
    }

    private void InitializeForegroundElement(Popup frame = null)
    {
        var stackPanel = Helper.CreateStackPanel(Orientation.Vertical, 5);
        var LayoutColorsRecent = Helper.CreateStackPanel();
        LayoutColorsRecent.Margin = new(0, 0, 0, 5);

        stackPanel.Children.Add(Helper.CreateLabel("По умолчанию"));
        var buttonDefault = ColorHelper.CreateButtonColor(Command, frame, Color.Parse(Brushes.Black.ToString()));
        buttonDefault.HorizontalAlignment = HorizontalAlignment.Left;
        stackPanel.Children.Add(buttonDefault);

        ColorHelper.CreateColorPalet(Command, frame, stackPanel);
        stackPanel.Children.Add(Helper.CreateLabel("Недавние цвета"));
        stackPanel.Children.Add(LayoutColorsRecent);

        frame.Opened += (_, _) => CreateColorsRecent(Command, frame, LayoutColorsRecent);
        var gridPanel = CreateGridPanel();
        var text = new TextBlock()
        {
            Text = "A",
            FontFamily = "Verdana",
            FontSize = 16,
            Margin = new(0, 0, 0, 0),
            Height = 20,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        Grid.SetRow(text, 0);
        gridPanel.Children.Add(text);

        Content = gridPanel;
        ContentPopup = new()
        {
            Width = 230,
            Children = { stackPanel }
        };
    }

    private void InitializeBorderStyleElement(Popup frame = null)
    {
        var stackPanel = Helper.CreateStackPanel(Orientation.Vertical, 5);
        stackPanel.Children.Add(Helper.CreateLabel("Границы"));
        GridStyleHelper.CreateGridStyle(Command, stackPanel, frame);

        ContentPopup ??= new()
        {
            Width = 230,
            Children = { stackPanel }
        };
    }


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (PickerType == SelectorPickerType.None || Content is { } && PickerType != SelectorPickerType.BorderStyle || !dictionaryColorPickerType.TryGetValue(PickerType, out var builder))
            return;

        var frame = FindPartById<Popup>(e, "PART_Popup");
        if (frame is { })
            builder.Invoke(frame);
    }

    private static Button CreateButtonClearColor(ICommand command, Popup frame)
    {
        var button = new Button()
        {
            Background = Brushes.Transparent,
            Padding = new(5),
            CornerRadius = new(0),
            BorderThickness = new(0),
            VerticalAlignment = VerticalAlignment.Stretch,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            HorizontalContentAlignment = HorizontalAlignment.Center,
            FontFamily = "Verdana",
            FontWeight = FontWeight.Bold,
            FontSize = 11,
            Content = "ОЧИСТИТЬ ЗАЛИВКУ",
            Command = command,
            CommandParameter = Brushes.Transparent
        };
        button.Classes.Add("highlightedBackground");

        button.Click += (_, _)
            => frame.Close();
        return button;
    }
    private void CreateColorsRecent(ICommand command, Popup popupFrame, StackPanel LayoutColorsRecent)
    {
        LayoutColorsRecent.Children.Clear();
        foreach (var color in BackgroundColors)
            LayoutColorsRecent.Children.Add(ColorHelper.CreateButtonColor(command, popupFrame, color));
    }
    private Grid CreateGridPanel()
    {
        var grid = new Grid()
        {
            RowDefinitions = new("* 5")
        };
        var border = new Border()
        {
            BorderThickness = new(1),
            BorderBrush = Brushes.Gray
        };

        if (CommandParameter is IBrush)
            border.Bind(Border.BackgroundProperty, new Binding() { Source = this, Path = "CommandParameter" });

        Grid.SetRow(border, 1);
        grid.Children.Add(border);
        return grid;
    }

    private void RenderGridStyle(CurrentBorderStyleType borderStyleType)
        => Content = GridStyleHelper.CreateGridStyle(borderStyleType);
}