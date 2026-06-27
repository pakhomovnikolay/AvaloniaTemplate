using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Controls;

public class ButtonClipboard : BaseTemplatedControl
{
    #region Тип взаимодействия с буфером обмена
    public static readonly StyledProperty<TemplatrButtonClipboardType> ClipboardTypeProperty =
        AvaloniaProperty.Register<ButtonClipboard, TemplatrButtonClipboardType>(nameof(ClipboardType));

    /// <summary>
    /// Тип взаимодействия с буфером обмена
    /// </summary>
    public TemplatrButtonClipboardType ClipboardType
    {
        get => GetValue(ClipboardTypeProperty);
        set => SetValue(ClipboardTypeProperty, value);
    }
    #endregion

    #region Путь к изображению
    internal static readonly StyledProperty<Bitmap> ImagePathProperty =
        AvaloniaProperty.Register<ButtonClipboard, Bitmap>(nameof(ImagePath));

    /// <summary>
    /// Путь к изображению
    /// </summary>
    internal Bitmap ImagePath
    {
        get => GetValue(ImagePathProperty);
        set => SetValue(ImagePathProperty, value);
    }
    #endregion

    #region Путь к изображению
    internal static readonly StyledProperty<Orientation?> OrientationTypeProperty =
        AvaloniaProperty.Register<ButtonClipboard, Orientation?>(nameof(OrientationType));

    /// <summary>
    /// Путь к изображению
    /// </summary>
    internal Orientation? OrientationType
    {
        get => GetValue(OrientationTypeProperty);
        set => SetValue(OrientationTypeProperty, value);
    }
    #endregion

    #region Заголовок кнопки
    internal static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<ButtonClipboard, string>(nameof(Header));

    /// <summary>
    /// Заголовок кнопки
    /// </summary>
    internal string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    #endregion

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, HorizontalAlignment>(nameof(HorizontalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<ComboBoxWithFonts, VerticalAlignment>(nameof(VerticalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ButtonClipboard, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<ButtonClipboard, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

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

        ImagePath ??= InitializeImage();
        Header ??= InitializeHeader();
        OrientationType ??= InitializeOrientation();
    }
    private Bitmap InitializeImage()
    {
        return ClipboardType switch
        {
            TemplatrButtonClipboardType.AsSimple => new Bitmap(AssetLoader.Open(new Uri($"avares://AvaloniaTemplate/Assets/AsSimple.png"))),
            TemplatrButtonClipboardType.Cut => new Bitmap(AssetLoader.Open(new Uri("avares://AvaloniaTemplate/Assets/Cut1.png"))),
            TemplatrButtonClipboardType.Paste => new Bitmap(AssetLoader.Open(new Uri("avares://AvaloniaTemplate/Assets/Paste.png"))),
            _ => new Bitmap(AssetLoader.Open(new Uri("avares://AvaloniaTemplate/Assets/Copy.png")))
        };
    }
    private string InitializeHeader()
    {
        return ClipboardType switch
        {
            TemplatrButtonClipboardType.AsSimple => "Формат по образцу",
            TemplatrButtonClipboardType.Cut => "Вырезать",
            TemplatrButtonClipboardType.Paste => "Вставить",
            _ => "Копировать"
        };
    }
    private Orientation InitializeOrientation()
    {
        return ClipboardType switch
        {
            TemplatrButtonClipboardType.Paste => Orientation.Vertical,
            _ => Orientation.Horizontal
        };
    }
}