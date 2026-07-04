using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Media;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class BorderWithTextBox : BaseTemplatedControl
{
    #region Шрифт заголовка
    public static readonly StyledProperty<FontFamily> HeaderFontFamilyProperty =
        AvaloniaProperty.Register<BorderWithTextBox, FontFamily>(
            nameof(HeaderFontFamily),
            defaultValue: "Verdana");

    /// <summary>
    /// Шрифт заголовка
    /// </summary>
    public FontFamily HeaderFontFamily
    {
        get => GetValue(HeaderFontFamilyProperty);
        set => SetValue(HeaderFontFamilyProperty, value);
    }
    #endregion

    #region Размер шрифта заголовка
    public static readonly StyledProperty<double> HeaderFontSizeProperty =
        AvaloniaProperty.Register<BorderWithTextBox, double>(
            nameof(HeaderFontSize),
            defaultValue: 11);

    /// <summary>
    /// Размер шрифта заголовка
    /// </summary>
    public double HeaderFontSize
    {
        get => GetValue(HeaderFontSizeProperty);
        set => SetValue(HeaderFontSizeProperty, value);
    }
    #endregion

    #region Толщина шрифта заголовка
    public static readonly StyledProperty<FontWeight> HeaderFontWeightProperty =
        AvaloniaProperty.Register<BorderWithTextBox, FontWeight>(
            nameof(HeaderFontWeight),
            defaultValue: FontWeight.Bold);

    /// <summary>
    /// Толщина шрифта заголовка
    /// </summary>
    public FontWeight HeaderFontWeight
    {
        get => GetValue(HeaderFontWeightProperty);
        set => SetValue(HeaderFontWeightProperty, value);
    }
    #endregion

    #region Заголовок
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<BorderWithTextBox, string>(
            nameof(Header),
            defaultValue: "Заголовок");

    /// <summary>
    /// Заголовок
    /// </summary>
    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    #endregion

    #region Водяной знак
    public static readonly StyledProperty<string> WatermarkProperty =
        AvaloniaProperty.Register<BorderWithTextBox, string>(
            nameof(Watermark),
            defaultValue: "Введите текст");

    /// <summary>
    /// Водяной знак
    /// </summary>
    public string Watermark
    {
        get => GetValue(WatermarkProperty);
        set => SetValue(WatermarkProperty, value);
    }
    #endregion

    #region Текст
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<BorderWithTextBox, string?>(
            nameof(Text),
            defaultValue: "215315",
            defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Текст
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
    #endregion

    #region Цвет заднего фона для воодимого текста
    public static readonly StyledProperty<IBrush?> BackgroundTextProperty =
        AvaloniaProperty.Register<BorderWithTextBox, IBrush?>(
            nameof(BackgroundText),
            defaultValue: Brushes.LightGray);

    /// <summary>
    /// Цвет заднего фона для воодимого текста
    /// </summary>
    public IBrush? BackgroundText
    {
        get => GetValue(BackgroundTextProperty);
        set => SetValue(BackgroundTextProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
    }
}