using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class SelectorScrollBar : BaseTemplatedControl
{
    #region Расположение полосы прокрутки
    public static readonly StyledProperty<Orientation> PositionProperty =
        AvaloniaProperty.Register<SelectorScrollBar, Orientation>(nameof(Position));

    /// <summary>
    /// Расположение полосы прокрутки
    /// </summary>
    public Orientation Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }
    #endregion

    #region Видимость панели полосы прокрутки
    public static readonly StyledProperty<ScrollBarVisibility> VisibilityProperty =
        AvaloniaProperty.Register<SelectorScrollBar, ScrollBarVisibility>(nameof(Visibility));

    /// <summary>
    /// Видимость панели полосы прокрутки
    /// </summary>
    public ScrollBarVisibility Visibility
    {
        get => GetValue(VisibilityProperty);
        set => SetValue(VisibilityProperty, value);
    }
    #endregion

    #region Текущее положение полосы прокрутки
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<SelectorScrollBar, double>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Текущее положение полосы прокрутки
    /// </summary>
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
    #endregion

    #region Событие изменения положения полосы прокрутки
    /// <summary>
    /// Событие изменения положения полосы прокрутки
    /// </summary>
    public event Action<double> ValueChanged;
    #endregion

    #region Скрывать автоматически
    public static readonly StyledProperty<bool> AllowAutoHideProperty =
        AvaloniaProperty.Register<SelectorScrollBar, bool>(nameof(AllowAutoHide));

    /// <summary>
    /// Скрывать автоматически
    /// </summary>
    public bool AllowAutoHide
    {
        get => GetValue(AllowAutoHideProperty);
        set => SetValue(AllowAutoHideProperty, value);
    }
    #endregion

    #region Максимальный размер контента
    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<SelectorScrollBar, double>(nameof(Maximum), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Максимальный размер контента
    /// </summary>
    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }
    #endregion

    #region Текущий размер области видимости
    public static readonly StyledProperty<double> ViewportSizeProperty =
        AvaloniaProperty.Register<SelectorScrollBar, double>(nameof(ViewportSize), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Текущий размер области видимости
    /// </summary>
    public double ViewportSize
    {
        get => GetValue(ViewportSizeProperty);
        set => SetValue(ViewportSizeProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (FindPartById<ScrollBar>(e, "PART_ScrollBar") is not { } scroll)
            return;

        scroll.ValueChanged += (_, e) => ValueChanged?.Invoke(scroll.Value);
    }
}