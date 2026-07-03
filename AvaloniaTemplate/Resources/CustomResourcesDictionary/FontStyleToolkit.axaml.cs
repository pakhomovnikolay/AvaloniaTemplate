using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class FontStyleToolkit : BaseTemplatedControl
{
    static FontStyleToolkit()
    {
        ToolkitTypeProperty.Changed.AddClassHandler<FontStyleToolkit>((x, _) => x.OnToolkitTypeChanged());
        IsCheckedProperty.Changed.AddClassHandler<FontStyleToolkit>((x, _) => x.OnIsCheckedChanged());
    }

    public FontStyleToolkit()
    {
        dictionaryToolkitType = new()
        {
            { TemplateButtonFontStyleToolkitType.FontWeightBold, () => InitializeFontStyle("Ж", FontWeight.Bold) },
            { TemplateButtonFontStyleToolkitType.FontStyleItalic, () => InitializeFontStyle("К", fontStyle: FontStyle.Italic) },
            { TemplateButtonFontStyleToolkitType.TextStyleUnderLine, () => InitializeFontStyle("Ч", IsUnderline: true) },
        };
    }

    #region Словарь типов инструментов
    /// <summary>
    /// Словарь типов инструментов
    /// </summary>
    private readonly Dictionary<TemplateButtonFontStyleToolkitType, Action> dictionaryToolkitType = [];
    #endregion

    #region Тип инструмента
    public static readonly StyledProperty<TemplateButtonFontStyleToolkitType> ToolkitTypeProperty =
        AvaloniaProperty.Register<FontStyleToolkit, TemplateButtonFontStyleToolkitType>(nameof(ToolkitType));

    /// <summary>
    /// Тип инструмента
    /// </summary>
    public TemplateButtonFontStyleToolkitType ToolkitType
    {
        get => GetValue(ToolkitTypeProperty);
        set => SetValue(ToolkitTypeProperty, value);
    }
    #endregion

    #region Контент
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<FontStyleToolkit, object?>(nameof(Content));

    /// <summary>
    /// Контент
    /// </summary>
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    #endregion

    #region Шаблон контента 
    public static readonly StyledProperty<IDataTemplate?> ContentTemplateProperty =
        AvaloniaProperty.Register<FontStyleToolkit, IDataTemplate?>(nameof(ContentTemplate));

    /// <summary>
    /// Шаблон контента 
    /// </summary>
    public IDataTemplate? ContentTemplate
    {
        get => GetValue(ContentTemplateProperty);
        set => SetValue(ContentTemplateProperty, value);
    }
    #endregion

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<FontStyleToolkit, HorizontalAlignment>(nameof(HorizontalContentAlignment));

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
        AvaloniaProperty.Register<FontStyleToolkit, VerticalAlignment>(nameof(VerticalContentAlignment));

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Установлена
    public static readonly StyledProperty<bool> IsCheckedProperty =
        AvaloniaProperty.Register<FontStyleToolkit, bool>(nameof(IsChecked));
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
        AvaloniaProperty.Register<FontStyleToolkit, ICommand>(nameof(Command));

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
        AvaloniaProperty.Register<FontStyleToolkit, object?>(nameof(CommandParameter));

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    private void OnToolkitTypeChanged()
    {
        if (ToolkitType == TemplateButtonFontStyleToolkitType.None || Content is { } || !dictionaryToolkitType.TryGetValue(ToolkitType, out var builder))
            return;

        builder.Invoke();
    }

    private void OnIsCheckedChanged()
    {
        if (IsChecked)
            Foreground = Helper.GetResource<IBrush>("ToggleButtonForegroundChecked");
        else
            Foreground = Helper.GetResource<IBrush>("ToggleButtonForeground");
        
    }

    private void InitializeFontStyle(
        string text,
        FontWeight fontWeight = FontWeight.Normal,
        FontStyle fontStyle = FontStyle.Normal,
        bool IsUnderline = false
        ) => Content = CreateTextBlock(text, fontWeight, fontStyle, IsUnderline);

    private TextBlock CreateTextBlock(string text, FontWeight fontWeight = FontWeight.Normal, FontStyle fontStyle = FontStyle.Normal, bool IsUnderline = false)
    {
        var textBlock = new TextBlock()
        {
            Text = text,
            FontWeight = fontWeight,
            FontStyle = fontStyle,
            IsHitTestVisible = false
        };
        textBlock.Bind(ForegroundProperty, new Binding() { Source = this, Path = "Foreground" });
        textBlock.Bind(FontFamilyProperty, new Binding() { Source = this, Path = "FontFamily" });
        textBlock.Bind(FontSizeProperty, new Binding() { Source = this, Path = "FontSize" });
        textBlock.Bind(HorizontalAlignmentProperty, new Binding() { Source = this, Path = "HorizontalAlignment" });
        textBlock.Bind(VerticalAlignmentProperty, new Binding() { Source = this, Path = "VerticalAlignment" });
        if (IsUnderline)
            textBlock.TextDecorations = TextDecorations.Underline;

        return textBlock;
    }
}