using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Infrastructures.Helpers;
using System;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class SliderZoomControl : TemplatedControl
{
    static SliderZoomControl()
    {
        ValueSliderProperty.Changed.AddClassHandler<SliderZoomControl>((x, _) => x.ValueSliderToString());
    }

    #region Окно раскрыто
    public static readonly StyledProperty<bool> IsPopupOpennedProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, bool>(nameof(IsPopupOpenned));

    /// <summary>
    /// Окно раскрыто
    /// </summary>
    public bool IsPopupOpenned
    {
        get => GetValue(IsPopupOpennedProperty);
        set => SetValue(IsPopupOpennedProperty, value);
    }
    #endregion

    #region Источник данных раскрывающегося окна
    public static readonly StyledProperty<Panel> PopupContentProperty =
        AvaloniaProperty.Register<DualListSelector, Panel>(nameof(PopupContent), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных раскрывающегося окна
    /// </summary>
    public Panel PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }
    #endregion

    #region Отображение кнопки увеления масштаба
    public static readonly StyledProperty<bool> IsVisibleButoonIncProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, bool>(
            nameof(IsVisibleButoonInc),
            defaultValue: true
            );

    /// <summary>
    /// Отображение кнопки увеления масштаба
    /// </summary>
    public bool IsVisibleButoonInc
    {
        get => GetValue(IsVisibleButoonIncProperty);
        set => SetValue(IsVisibleButoonIncProperty, value);
    }
    #endregion

    #region Отображение кнопки уменьшения масштаба
    public static readonly StyledProperty<bool> IsVisibleButoonDeincProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, bool>(
            nameof(IsVisibleButoonDeinc),
            defaultValue: true
            );

    /// <summary>
    /// Отображение кнопки уменьшения масштаба
    /// </summary>
    public bool IsVisibleButoonDeinc
    {
        get => GetValue(IsVisibleButoonDeincProperty);
        set => SetValue(IsVisibleButoonDeincProperty, value);
    }
    #endregion

    #region Ширина панели бегунка
    public static readonly StyledProperty<double> WidthSliderPanelProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, double>(
            nameof(WidthSliderPanel),
            defaultValue: 150
            );

    /// <summary>
    /// Ширина панели бегунка
    /// </summary>
    public double WidthSliderPanel
    {
        get => GetValue(WidthSliderPanelProperty);
        set => SetValue(WidthSliderPanelProperty, value);
    }
    #endregion

    #region Минимальнео значени бегунка
    public static readonly StyledProperty<double> MinimumSliderProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, double>(
            nameof(MinimumSlider),
            defaultValue: 0
            );

    /// <summary>
    /// Минимальнео значени бегунка
    /// </summary>
    public double MinimumSlider
    {
        get => GetValue(MinimumSliderProperty);
        set => SetValue(MinimumSliderProperty, value);
    }
    #endregion

    #region Максимальное значени бегунка
    public static readonly StyledProperty<double> MaximumSliderProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, double>(
            nameof(MaximumSlider),
            defaultValue: 3
            );

    /// <summary>
    /// Максимальное значени бегунка
    /// </summary>
    public double MaximumSlider
    {
        get => GetValue(MaximumSliderProperty);
        set => SetValue(MaximumSliderProperty, value);
    }
    #endregion

    #region Минимальное ограниченное значени бегунка
    public static readonly StyledProperty<double> MinStopValueSliderProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, double>(
            nameof(MinStopValueSlider),
            defaultValue: 0.1
            );

    /// <summary>
    /// Минимальное ограниченное значени бегунка
    /// </summary>
    public double MinStopValueSlider
    {
        get => GetValue(MinStopValueSliderProperty);
        set => SetValue(MinStopValueSliderProperty, value);
    }
    #endregion

    #region Минимальная длина шага
    public static readonly StyledProperty<double> SmallChangeSliderProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, double>(
            nameof(SmallChangeSlider),
            defaultValue: 0.5
            );

    /// <summary>
    /// Минимальная длина шага
    /// </summary>
    public double SmallChangeSlider
    {
        get => GetValue(SmallChangeSliderProperty);
        set => SetValue(SmallChangeSliderProperty, value);
    }
    #endregion

    #region Максимальная длина шага
    public static readonly StyledProperty<double> LargeChangeSliderProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, double>(
            nameof(LargeChangeSlider),
            defaultValue: 1.5
            );

    /// <summary>
    /// Максимальная длина шага
    /// </summary>
    public double LargeChangeSlider
    {
        get => GetValue(LargeChangeSliderProperty);
        set => SetValue(LargeChangeSliderProperty, value);
    }
    #endregion

    #region Текущее значение бегунка
    public static readonly StyledProperty<double> ValueSliderProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, double>(
            nameof(ValueSlider),
            defaultValue: 1,
            defaultBindingMode: BindingMode.TwoWay
            );

    /// <summary>
    /// Текущее значение бегунка
    /// </summary>
    public double ValueSlider
    {
        get => GetValue(ValueSliderProperty);
        set => SetValue(ValueSliderProperty, value);
    }
    #endregion

    #region Текущее значение бегунка для отображения
    public static readonly StyledProperty<string> VisualValueSliderProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, string>(
            nameof(VisualValueSlider),
            defaultValue: "100 %",
            defaultBindingMode: BindingMode.TwoWay
            );

    /// <summary>
    /// Текущее значение бегунка для отображения
    /// </summary>
    public string VisualValueSlider
    {
        get => GetValue(VisualValueSliderProperty);
        set => SetValue(VisualValueSliderProperty, value);
    }
    #endregion

    #region Текущее значение бегунка для редактирования
    public static readonly StyledProperty<int> EditValueSliderProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, int>(
            nameof(EditValueSlider),
            defaultValue: 1,
            defaultBindingMode: BindingMode.TwoWay
            );

    /// <summary>
    /// Текущее значение бегунка для редактирования
    /// </summary>
    public int EditValueSlider
    {
        get => GetValue(EditValueSliderProperty);
        set => SetValue(EditValueSliderProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var buttonZoomDeinc = e.NameScope.Find<Button>("PART_ButtonZoomDeinc");
        var buttonZoomInc = e.NameScope.Find<Button>("PART_ButtonZoomInc");

        buttonZoomDeinc.Click += (_, _) =>
        {
            if (ValueSlider > MinStopValueSlider)
                ValueSlider -= SmallChangeSlider / 10;
        };

        buttonZoomInc.Click += (_, _) =>
        {
            if (ValueSlider < MaximumSlider)
                ValueSlider += SmallChangeSlider / 10;
        };

        EditValueSlider = Convert.ToInt32(ValueSlider * 100);
        var writeValueSlider = new TextBox()
        {
            Margin = new(5),
            Padding = new(3, 0, 0, 0),
            Height = 22,
            VerticalContentAlignment = VerticalAlignment.Center
        };
        writeValueSlider.Bind(TextBox.TextProperty, new Binding("EditValueSlider") { Source = this });
        TextBoxHelper.SetEnterKeyCommand(writeValueSlider, EditValueSliderFiniched);

        PopupContent = new()
        {
            Children =
            {
                new StackPanel()
                {
                    Spacing = 5,
                    Children = { new TextBlock() { Text = "Свой масштаб:", Margin = new(5, 5, 0, 0) }, writeValueSlider }
                }
            }
        };
    }

    private void ValueSliderToString()
    {
        if (ValueSlider < MinStopValueSlider)
        {
            MinimumSlider = MinStopValueSlider;
            ValueSlider = MinStopValueSlider;
        }
        else if (ValueSlider - MinStopValueSlider > MinStopValueSlider)
            MinimumSlider = 0;

        VisualValueSlider = $"{Convert.ToInt32(ValueSlider * 100)} %";
        EditValueSlider = Convert.ToInt32(ValueSlider * 100);
    }

    private ICommand EditValueSliderFiniched
        => new RelayCommand(ExecuteEditValueSliderFiniched);

    private void ExecuteEditValueSliderFiniched()
    {
        ValueSlider = Convert.ToInt32(EditValueSlider / 100);
        IsPopupOpenned = false;
    }
}