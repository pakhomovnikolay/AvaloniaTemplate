using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System.Collections;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Controls;

public class ButtonFontStyleToolkit : BaseTemplatedControl
{
    #region Тип инструмента
    public static readonly StyledProperty<TemplateButtonFontStyleToolkitType> ToolkitTypeProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, TemplateButtonFontStyleToolkitType>(nameof(ToolkitType));

    /// <summary>
    /// Тип инструмента
    /// </summary>
    public TemplateButtonFontStyleToolkitType ToolkitType
    {
        get => GetValue(ToolkitTypeProperty);
        set => SetValue(ToolkitTypeProperty, value);
    }
    #endregion

    #region Источник данных
    internal static readonly StyledProperty<IList?> ItemsSourceProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, IList?>(nameof(ItemsSource), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных
    /// </summary>
    internal IList? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion

    #region Выбранный элемент из спсика
    internal static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент из спсика
    /// </summary>
    internal object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    #endregion

    #region Индекс выбранного элемента из спсика
    internal static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, int>(nameof(SelectedIndex), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Индекс выбранного элемента из спсика
    /// </summary>
    internal int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }
    #endregion

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, HorizontalAlignment>(nameof(HorizontalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<ButtonFontStyleToolkit, VerticalAlignment>(nameof(VerticalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Минимальная ширина кнопок смены размера шрифта
    public static readonly StyledProperty<double> MinWidthButtonSizeProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, double>(nameof(MinWidthButtonSize), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Минимальная ширина кнопок смены размера шрифта
    /// </summary>
    public double MinWidthButtonSize
    {
        get => GetValue(MinWidthButtonSizeProperty);
        set => SetValue(MinWidthButtonSizeProperty, value);
    }
    #endregion

    #region Минимальная высота кнопок смены размера шрифта
    public static readonly StyledProperty<double> MinHeightButtonSizeProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, double>(nameof(MinHeightButtonSize));

    /// <summary>
    /// Минимальная высота кнопок смены размера шрифта
    /// </summary>
    public double MinHeightButtonSize
    {
        get => GetValue(MinHeightButtonSizeProperty);
        set => SetValue(MinHeightButtonSizeProperty, value);
    }
    #endregion

    #region Установлена
    public static readonly StyledProperty<bool> IsCheckedProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, bool>(nameof(IsChecked), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Установлена
    /// </summary>
    public bool IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    #endregion

    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ButtonFontStyleToolkit, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<ButtonFontStyleToolkit, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion
    
    
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (ToolkitType == TemplateButtonFontStyleToolkitType.FontList)
            InitializeFontList(e);
        else if (ToolkitType == TemplateButtonFontStyleToolkitType.FontSizeList)
            InitializeFontSizeList(e);
        else if (ToolkitType != TemplateButtonFontStyleToolkitType.None)
            InitializeButtons(e);
    }

    private void InitializeFontList(TemplateAppliedEventArgs e)
    {
        ItemsSource = FontFamilyHelper.FontFamilies;
        SelectedItem = FontFamilyHelper.AppFontDefault ?? FontFamilyHelper.FontDefault;
        SelectedIndex = FontFamilyHelper.FontFamilies.IndexOf(FontFamilyHelper.FontDefault);
        var CBox = FindPartById<ComboBox>(e, "PART_FontListPanel");
        if (CBox is { })
        {
            CBox.IsVisible = true;
        }
    }

    private void InitializeFontSizeList(TemplateAppliedEventArgs e)
    {
        ItemsSource = FontFamilyHelper.FontSizes;
        SelectedItem = FontFamilyHelper.FontSizeDefault;
        SelectedIndex = FontFamilyHelper.FontSizes.IndexOf(FontFamilyHelper.FontSizeDefault);
        var panel = FindPartById<Grid>(e, "PART_FontSizeListPanel");
        if (panel is { })
        {
            panel.IsVisible = true;
        }
    }

    private void InitializeButtons(TemplateAppliedEventArgs e)
    {
        var nameButton = ToolkitType switch
        {
            TemplateButtonFontStyleToolkitType.TextStyleUnderLine => "PART_ButtonTextStyleUnderLine",
            TemplateButtonFontStyleToolkitType.FontStyleItalic => "PART_ButtonFontStyleItalic",
            _ => "PART_ButtonFontWeightBold"
        };
        var button = FindPartById<ToggleButton>(e, nameButton);
        if (button is { })
        {
            button.IsVisible = true;
        }
    }
}