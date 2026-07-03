using Avalonia;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ButtonClipboard : BaseTemplatedControl
{
    private Color defaultBackground;
    private Color defaultForeground;

    static ButtonClipboard()
    {
        ClipboardTypeProperty.Changed.AddClassHandler<ButtonClipboard>((x, _) => x.OnClipboardTypeChanged());
        IsCheckedProperty.Changed.AddClassHandler<ButtonClipboard>((x, _) => x.OnIsAsSimpleFixedChanged());
    }

    #region Словарь типов взаимодействия с буфером обмена
    /// <summary>
    /// Словарь типов взаимодействия с буфером обмена
    /// </summary>
    private static readonly Dictionary<TemplatrButtonClipboardType, (Uri imagePath, string header, Orientation orientationType)> dictionaryClipboardType = new()
    {
        { TemplatrButtonClipboardType.Paste, (new Uri("avares://AvaloniaTemplate/Assets/Paste.png"), "Вставить", Orientation.Vertical) },
        { TemplatrButtonClipboardType.Copy, (new Uri("avares://AvaloniaTemplate/Assets/Copy.png"), "Копировать", Orientation.Horizontal) },
        { TemplatrButtonClipboardType.Cut, (new Uri("avares://AvaloniaTemplate/Assets/Cut.png"), "Вырезать", Orientation.Horizontal) },
        { TemplatrButtonClipboardType.AsSimple, (new Uri("avares://AvaloniaTemplate/Assets/AsSimple.png"), "Формат по образцу", Orientation.Horizontal) }
    };
    #endregion

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

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<ButtonClipboard, HorizontalAlignment>(nameof(HorizontalContentAlignment));

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
        AvaloniaProperty.Register<ButtonClipboard, VerticalAlignment>(nameof(VerticalContentAlignment));

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Расположение объектов
    public static readonly StyledProperty<Orientation?> OrientationTypeProperty =
        AvaloniaProperty.Register<ButtonClipboard, Orientation?>(nameof(OrientationType));

    /// <summary>
    /// Расположение объектов
    /// </summary>
    public Orientation? OrientationType
    {
        get => GetValue(OrientationTypeProperty);
        set => SetValue(OrientationTypeProperty, value);
    }
    #endregion

    #region Расстояние между элементами
    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<ButtonClipboard, double>(nameof(Spacing));

    /// <summary>
    /// Расстояние между элементами
    /// </summary>
    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }
    #endregion

    #region Источник изображения
    public static readonly StyledProperty<Bitmap?> ImageSourceProperty =
        AvaloniaProperty.Register<ButtonClipboard, Bitmap?>(nameof(ImageSource));

    /// <summary>
    /// Источник изображения
    /// </summary>
    public Bitmap? ImageSource
    {
        get => GetValue(ImageSourceProperty);
        set => SetValue(ImageSourceProperty, value);
    }
    #endregion

    #region Заголовок кнопки
    public static readonly StyledProperty<string?> HeaderProperty =
        AvaloniaProperty.Register<ButtonClipboard, string?>(nameof(Header));

    /// <summary>
    /// Заголовок кнопки
    /// </summary>
    public string? Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }
    #endregion

    #region Расстояние между элементами
    public static readonly StyledProperty<double> ImageHeightProperty =
        AvaloniaProperty.Register<ButtonClipboard, double>(nameof(ImageHeight));

    /// <summary>
    /// Расстояние между элементами
    /// </summary>
    public double ImageHeight
    {
        get => GetValue(ImageHeightProperty);
        set => SetValue(ImageHeightProperty, value);
    }
    #endregion

    #region Расстояние между элементами
    public static readonly StyledProperty<double> ImageWidthProperty =
        AvaloniaProperty.Register<ButtonClipboard, double>(nameof(ImageWidth));

    /// <summary>
    /// Расстояние между элементами
    /// </summary>
    public double ImageWidth
    {
        get => GetValue(ImageWidthProperty);
        set => SetValue(ImageWidthProperty, value);
    }
    #endregion

    #region Установлена
    public static readonly StyledProperty<bool> IsCheckedProperty =
        AvaloniaProperty.Register<ButtonMergeCells, bool>(nameof(IsChecked), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<ButtonClipboard, ICommand>(nameof(Command));

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
        AvaloniaProperty.Register<ButtonClipboard, object?>(nameof(CommandParameter));

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    private void OnClipboardTypeChanged()
    {
        if (ClipboardType == TemplatrButtonClipboardType.None || !dictionaryClipboardType.TryGetValue(ClipboardType, out var value))
            return;

        defaultBackground = Color.Parse(Helper.GetColor(Background) ?? Brushes.Transparent.ToString());
        defaultForeground = Color.Parse(Helper.GetColor(Background) ?? Brushes.Black.ToString());
        ImageSource ??= new Bitmap(AssetLoader.Open(value.imagePath));
        Header ??= value.header;
        OrientationType ??= value.orientationType;

        if (ClipboardType == TemplatrButtonClipboardType.AsSimple)
            Tapped += (_, _) => IsChecked = !IsChecked;
    }

    private void OnIsAsSimpleFixedChanged()
    {
        Background = IsChecked ? Helper.GetResource<IBrush>("ToggleButtonBackgroundChecked") : new SolidColorBrush(defaultBackground);
        Foreground = IsChecked ? Helper.GetResource<IBrush>("ToggleButtonForegroundChecked") : new SolidColorBrush(defaultForeground);
    }
}