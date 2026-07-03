using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Collections;
using System.Collections.Generic;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class FontSelectorToolkit : BaseTemplatedControl
{
    private Panel? rootPanel;
    private ComboBox? comboBox;
    private Button? buttonFontSizeUp;
    private Button? buttonFontSizeDown;

    public FontSelectorToolkit()
    {
        dictionaryToolkitType = new()
        {
            { TemplateFontSelectorToolkitType.FontList, InitializeFontList },
            { TemplateFontSelectorToolkitType.FontSizeList, InitializeFontSizeList }
        };
    }

    #region Словарь типов инструментов
    /// <summary>
    /// Словарь типов инструментов
    /// </summary>
    private readonly Dictionary<TemplateFontSelectorToolkitType, Action> dictionaryToolkitType = [];
    #endregion

    #region Тип инструмента
    public static readonly StyledProperty<TemplateFontSelectorToolkitType> ToolkitTypeProperty =
        AvaloniaProperty.Register<FontSelectorToolkit, TemplateFontSelectorToolkitType>(nameof(ToolkitType));

    /// <summary>
    /// Тип инструмента
    /// </summary>
    public TemplateFontSelectorToolkitType ToolkitType
    {
        get => GetValue(ToolkitTypeProperty);
        set => SetValue(ToolkitTypeProperty, value);
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<IList?> ItemsSourceProperty =
        AvaloniaProperty.Register<FontSelectorToolkit, IList?>(nameof(ItemsSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public IList? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion

    #region Выбранный элемент из спсика
    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<FontSelectorToolkit, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент из спсика
    /// </summary>
    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    #endregion

    #region Индекс выбранного элемента из спсика
    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<FontSelectorToolkit, int>(nameof(SelectedIndex), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Индекс выбранного элемента из спсика
    /// </summary>
    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }
    #endregion

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<FontSelectorToolkit, HorizontalAlignment>(nameof(HorizontalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<FontSelectorToolkit, VerticalAlignment>(nameof(VerticalContentAlignment));

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Событие изменения предпросмотра
    /// <summary>
    /// Событие изменения предпросмотра
    /// </summary>
    public event Action<object?> PreviewChanged;
    #endregion

    #region Событие изменения текущего элемента
    /// <summary>
    /// Событие изменения текущего элемента
    /// </summary>
    public event Action<object?> SelectedItemChanged;
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        rootPanel = FindPartById<Panel>(e, "PART_RootPanel");
        comboBox = FindPartById<ComboBox>(e, "PART_ComboBox");
        if (ToolkitType == TemplateFontSelectorToolkitType.None
            || rootPanel is not { }
            || comboBox is not { }
            || !dictionaryToolkitType.TryGetValue(ToolkitType, out var builder))
            return;

        builder.Invoke();
        comboBox.SelectionChanged -= ComboBox_SelectionChanged;
        comboBox.SelectionChanged += ComboBox_SelectionChanged;
    }

    private void ComboBox_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        SelectedItemChanged?.Invoke(SelectedItem);
    }

    private void InitializeFontList()
    {
        ItemsSource = FontFamilyHelper.FontFamilies;
        SelectedItem = FontFamilyHelper.AppFontDefault ?? FontFamilyHelper.FontDefault;
        comboBox.ItemTemplate = new FuncDataTemplate<FontFamily>((item, _) =>
        {
            var fontFamily = item ?? FontFamilyHelper.FontDefault;
            return CreateBorderItemTemplate(fontFamily.Name, fontFamily, fontFamily);
        });
    }

    private void InitializeFontSizeList()
    {
        ItemsSource = FontFamilyHelper.FontSizes;
        SelectedItem = FontFamilyHelper.FontSizeDefault;
        comboBox.ItemTemplate = new FuncDataTemplate<double>((item, _) =>
        {
            var fontFamily = FontFamilyHelper.AppFontDefault ?? FontFamilyHelper.FontDefault;
            return CreateBorderItemTemplate($"{item}", fontFamily, item);
        });

        var margin = new Thickness(0, 0, 30, 0);
        buttonFontSizeUp ??= CreateButtonSetFontSizeUp(margin);
        if (!rootPanel.Children.Contains(buttonFontSizeUp))
            rootPanel.Children.Add(buttonFontSizeUp);

        buttonFontSizeDown ??= CreateButtonSetFontSizeDown(margin);
        if (!rootPanel.Children.Contains(buttonFontSizeDown))
            rootPanel.Children.Add(buttonFontSizeDown);

        comboBox.Margin = new(margin.Left, margin.Top, margin.Right * 2 + 3, margin.Bottom);
    }

    private Border CreateBorderItemTemplate(string text, FontFamily fontFamily, object? itemInvoke)
    {
        var border = new Border
        {
            Background = Brushes.Transparent,
            BorderThickness = new Thickness(0),
            Padding = new Thickness(0),
            Child = new TextBlock
            {
                IsHitTestVisible = false,
                Text = text,
                FontFamily = fontFamily
            }
        };
        border.PointerEntered += (_, _) => PreviewChanged?.Invoke(itemInvoke);
        border.PointerExited += (_, _) => PreviewChanged?.Invoke(null);
        return border;
    }

    private Button CreateButtonSetFontSizeUp(Thickness margin)
    {
        var StreamGeometryData = "M1939 1401L2029 1311L1024 306L19 1311L109 1401L1024 486L1939 1401Z";
        var marginButton = new Thickness(margin.Left, margin.Top, margin.Right * 1, margin.Bottom);
        var marginPathIcon = new Thickness(10, 0, 0, 9);
        var fonSize = 16;
        var button = CreateButtonChangeFontSize(StreamGeometryData, marginButton, marginPathIcon, fonSize);
        button.Click += (s, e) =>
        {
            if (SelectedIndex < comboBox.ItemCount - 1)
                SelectedIndex++;
        };
        return button;
    }

    private Button CreateButtonSetFontSizeDown(Thickness margin)
    {
        var StreamGeometryData = "M1939 486L2029 576L1024 1581L19 576L109 486L1024 1401L1939 486Z";
        var marginButton = new Thickness(margin.Left, margin.Top, margin.Right * 0, margin.Bottom);
        var marginPathIcon = new Thickness(8, 0, 0, 3);
        var fonSize = 12;
        var button = CreateButtonChangeFontSize(StreamGeometryData, marginButton, marginPathIcon, fonSize);
        button.Click += (s, e) =>
        {
            if (SelectedIndex > 0)
                SelectedIndex--;
        };
        return button;
    }

    private static Button CreateButtonChangeFontSize(string StreamGeometryData, Thickness marginButton, Thickness marginPathIcon, double fontSize)
    {
        var button = new Button()
        {
            MinWidth = 30,
            MinHeight = 25,
            Padding = new(),
            Margin = marginButton,
            Background = Brushes.Transparent,
            BorderThickness = new(0),
            BorderBrush = Brushes.Gray,
            HorizontalAlignment = HorizontalAlignment.Right,
            VerticalAlignment = VerticalAlignment.Stretch,
            Content = new Panel()
            {
                Children =
                {
                    new TextBlock()
                    {
                        Text = "A",
                        FontSize = fontSize,
                        FontFamily = FontFamilyHelper.AppFontDefault ?? FontFamilyHelper.FontDefault,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment= VerticalAlignment.Bottom,
                        Margin = new(0, 0, 5, 3)
                    },
                    new PathIcon()
                    {
                        Height = 7,
                        Width = 7,
                        Margin = marginPathIcon,
                        Data = StreamGeometry.Parse(StreamGeometryData)
                    }
                }
            }
        };
        return button;
    }
}