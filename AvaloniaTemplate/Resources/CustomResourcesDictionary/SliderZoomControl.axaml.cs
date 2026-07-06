using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class SliderZoomControl : BaseTemplatedControl
{
    /// <summary>
    /// Предыдущее значени бегунка
    /// </summary>
    private readonly double MinStopValueSlider = 0.1;

    /// <summary>
    /// Предыдущее значени бегунка
    /// </summary>
    private double valueSlider;

    static SliderZoomControl()
    {
        ValueSliderProperty.Changed.AddClassHandler<SliderZoomControl>((x, _) => x.OnValueSliderChanged());
    }

    #region Отображение кнопки уменьшения масштаба
    public static readonly StyledProperty<bool> IsVisibleButoonDeincProperty =
        AvaloniaProperty.Register<SliderZoomControl, bool>(nameof(IsVisibleButoonDeinc), defaultValue: true);

    /// <summary>
    /// Отображение кнопки уменьшения масштаба
    /// </summary>
    public bool IsVisibleButoonDeinc
    {
        get => GetValue(IsVisibleButoonDeincProperty);
        set => SetValue(IsVisibleButoonDeincProperty, value);
    }
    #endregion

    #region Отображение кнопки увеления масштаба
    public static readonly StyledProperty<bool> IsVisibleButoonIncProperty =
        AvaloniaProperty.Register<SliderZoomControl, bool>(nameof(IsVisibleButoonInc), defaultValue: true);

    /// <summary>
    /// Отображение кнопки увеления масштаба
    /// </summary>
    public bool IsVisibleButoonInc
    {
        get => GetValue(IsVisibleButoonIncProperty);
        set => SetValue(IsVisibleButoonIncProperty, value);
    }
    #endregion

    #region Минимальнео значени бегунка
    public static readonly StyledProperty<double> MinimumSliderProperty =
        AvaloniaProperty.Register<SliderZoomControl, double>(nameof(MinimumSlider), defaultValue: 0);

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
        AvaloniaProperty.Register<SliderZoomControl, double>(nameof(MaximumSlider), defaultValue: 3);

    /// <summary>
    /// Максимальное значени бегунка
    /// </summary>
    public double MaximumSlider
    {
        get => GetValue(MaximumSliderProperty);
        set => SetValue(MaximumSliderProperty, value);
    }
    #endregion

    #region Минимальная длина шага
    public static readonly StyledProperty<double> SmallChangeSliderProperty =
        AvaloniaProperty.Register<SliderZoomControl, double>(nameof(SmallChangeSlider), defaultValue: 0.5);

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
        AvaloniaProperty.Register<SliderZoomControl, double>(nameof(LargeChangeSlider), defaultValue: 1.5);

    /// <summary>
    /// Максимальная длина шага
    /// </summary>
    public double LargeChangeSlider
    {
        get => GetValue(LargeChangeSliderProperty);
        set => SetValue(LargeChangeSliderProperty, value);
    }
    #endregion

    #region Ширина панели бегунка
    public static readonly StyledProperty<double> WidthSliderPanelProperty =
        AvaloniaProperty.Register<SliderZoomControl, double>(nameof(WidthSliderPanel), defaultValue: 150);

    /// <summary>
    /// Ширина панели бегунка
    /// </summary>
    public double WidthSliderPanel
    {
        get => GetValue(WidthSliderPanelProperty);
        set => SetValue(WidthSliderPanelProperty, value);
    }
    #endregion

    #region Текущее значение бегунка
    public static readonly StyledProperty<double> ValueSliderProperty =
        AvaloniaProperty.Register<SliderZoomControl, double>(nameof(ValueSlider), defaultValue: 0, defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<SliderZoomControl, string>(nameof(VisualValueSlider), defaultValue: "100 %", defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Текущее значение бегунка для отображения
    /// </summary>
    public string VisualValueSlider
    {
        get => GetValue(VisualValueSliderProperty);
        set => SetValue(VisualValueSliderProperty, value);
    }
    #endregion

    #region Значение бегунка для редактирования
    public static readonly StyledProperty<int> EditValueSliderProperty =
        AvaloniaProperty.Register<SliderZoomControl, int>(nameof(EditValueSlider), 0, defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Значение бегунка для редактирования
    /// </summary>
    public int EditValueSlider
    {
        get => GetValue(EditValueSliderProperty);
        set => SetValue(EditValueSliderProperty, value);
    }
    #endregion

    #region Окно раскрыто
    public static readonly StyledProperty<bool> IsPopupOpenProperty =
        AvaloniaProperty.Register<SliderZoomControl, bool>(nameof(IsPopupOpen), defaultBindingMode: BindingMode.TwoWay);

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
    public static readonly StyledProperty<Panel> ContentPopupProperty =
        AvaloniaProperty.Register<SliderZoomControl, Panel>(nameof(ContentPopup), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных раскрывающегося окна
    /// </summary>
    public Panel ContentPopup
    {
        get => GetValue(ContentPopupProperty);
        set => SetValue(ContentPopupProperty, value);
    }
    #endregion

    #region Расположение насечек
    public static readonly StyledProperty<TickPlacement> TickPlacementTypeProperty =
        AvaloniaProperty.Register<SliderZoomControl, TickPlacement>(nameof(TickPlacementType), defaultValue: TickPlacement.TopLeft, defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Расположение насечек
    /// </summary>
    public TickPlacement TickPlacementType
    {
        get => GetValue(TickPlacementTypeProperty);
        set => SetValue(TickPlacementTypeProperty, value);
    }
    #endregion

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<SliderZoomControl, HorizontalAlignment>(nameof(HorizontalContentAlignment));

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
        AvaloniaProperty.Register<SliderZoomControl, VerticalAlignment>(nameof(VerticalContentAlignment));

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Событие изменения положения бегунка
    /// <summary>
    /// Событие изменения положения бегунка
    /// </summary>
    public event Action<double> ValueChanged;
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        OnValueSliderChanged();
        CreateContentPopup();

        var buttonZoomDeinc = FindPartById<Button>(e, "PART_ButtonZoomDeinc");
        buttonZoomDeinc.Click += (_, _) =>
        {
            if (ValueSlider > MinStopValueSlider)
                ValueSlider -= SmallChangeSlider / 10;
        };


        var buttonZoomInc = FindPartById<Button>(e, "PART_ButtonZoomInc");
        buttonZoomInc.Click += (_, _) =>
        {
            if (ValueSlider < MaximumSlider)
                ValueSlider += SmallChangeSlider / 10;
        };

        var slider = FindPartById<Slider>(e, "PART_Slider");
        slider.PointerWheelChanged += (_, e) =>
        {
            ValueSlider += e.Delta.Y * (SmallChangeSlider / 10);
        };
    }

    private void OnValueSliderChanged()
    {
        if (valueSlider == ValueSlider)
            return;

        if (ValueSlider < MinStopValueSlider)
        {
            MinimumSlider = MinStopValueSlider;
            ValueSlider = MinStopValueSlider;
        }
        else if (ValueSlider - MinStopValueSlider > MinStopValueSlider && MinimumSlider > 0)
            MinimumSlider = 0;

        EditValueSlider = Convert.ToInt32(ValueSlider * 100);
        VisualValueSlider = $"{EditValueSlider} %";
        valueSlider = ValueSlider;
        ValueChanged?.Invoke(valueSlider);
    }

    private void CreateContentPopup()
    {
        var label = new TextBlock()
        {
            Text = "Свой масштаб:",
            Margin = new(5, 0, 0, 0),
            FontFamily = FontFamily,
            FontSize = FontSize
        };
        var textBoxEditValueSlider = new TextBox()
        {
            Margin = new(0),
            Padding = new(3, 0, 0, 0),
            Height = 22,
            VerticalContentAlignment = VerticalAlignment.Center,
            MinWidth = 150,
            FontFamily = FontFamily,
            FontSize = FontSize,
        };
        textBoxEditValueSlider.Bind(TextBox.TextProperty, new Binding("EditValueSlider") { Source = this });
        TextBoxHelper.SetEnterKeyCommand(textBoxEditValueSlider, EditValueSliderFinished);

        ContentPopup = new()
        {
            Children =
            {
                new StackPanel()
                {
                    Spacing = 5,
                    Children = { label , textBoxEditValueSlider }
                }
            }
        };
    }

    private ICommand EditValueSliderFinished
        => new RelayCommand(ExecuteEditValueSliderFiniched);

    private void ExecuteEditValueSliderFiniched()
    {
        ValueSlider = EditValueSlider / 100.0;
        IsPopupOpen = false;
    }
























    //#region Текущее значение бегунка для редактирования
    //public static readonly StyledProperty<int> EditValueSliderProperty =
    //    AvaloniaProperty.Register<SliderZoomControl, int>(
    //        nameof(EditValueSlider),
    //        defaultValue: 1,
    //        defaultBindingMode: BindingMode.TwoWay
    //        );

    ///// <summary>
    ///// Текущее значение бегунка для редактирования
    ///// </summary>
    //public int EditValueSlider
    //{
    //    get => GetValue(EditValueSliderProperty);
    //    set => SetValue(EditValueSliderProperty, value);
    //}
    //#endregion

    //protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    //{
    //    //base.OnApplyTemplate(e);

    //    //var buttonZoomDeinc = FindPartById<Button>(e, "PART_ButtonZoomDeinc");
    //    //var buttonZoomInc = FindPartById<Button>(e, "PART_ButtonZoomInc");

    //    //buttonZoomDeinc.Click += (_, _) =>
    //    //{
    //    //    if (ValueSlider > MinStopValueSlider)
    //    //        ValueSlider -= SmallChangeSlider / 10;
    //    //};

    //    //buttonZoomInc.Click += (_, _) =>
    //    //{
    //    //    if (ValueSlider < MaximumSlider)
    //    //        ValueSlider += SmallChangeSlider / 10;
    //    //};

    //    //EditValueSlider = Convert.ToInt32(ValueSlider * 100);
    //    //var writeValueSlider = new TextBox()
    //    //{
    //    //    Margin = new(5),
    //    //    Padding = new(3, 0, 0, 0),
    //    //    Height = 22,
    //    //    VerticalContentAlignment = VerticalAlignment.Center
    //    //};
    //    //writeValueSlider.Bind(TextBox.TextProperty, new Binding("EditValueSlider") { Source = this });
    //    //TextBoxHelper.SetEnterKeyCommand(writeValueSlider, EditValueSliderFiniched);

    //    //PopupContent = new()
    //    //{
    //    //    Children =
    //    //    {
    //    //        new StackPanel()
    //    //        {
    //    //            Spacing = 5,
    //    //            Children = { new TextBlock() { Text = "Свой масштаб:", Margin = new(5, 5, 0, 0) }, writeValueSlider }
    //    //        }
    //    //    }
    //    //};

    //    //var slider = FindPartById<Slider>(e, "PART_Slider");
    //    //slider.PointerWheelChanged += (_, e) =>
    //    //{
    //    //    ValueSlider += e.Delta.Y * (SmallChangeSlider / 10);
    //    //};
    //}

    ////private void ValueSliderToString()
    ////{
    ////    if (ValueSlider < MinStopValueSlider)
    ////    {
    ////        MinimumSlider = MinStopValueSlider;
    ////        ValueSlider = MinStopValueSlider;
    ////    }
    ////    else if (ValueSlider - MinStopValueSlider > MinStopValueSlider)
    ////        MinimumSlider = 0;

    ////    VisualValueSlider = $"{Convert.ToInt32(ValueSlider * 100)} %";
    ////    EditValueSlider = Convert.ToInt32(ValueSlider * 100);
    ////}

    ////private ICommand EditValueSliderFiniched
    ////    => new RelayCommand(ExecuteEditValueSliderFiniched);

    ////private void ExecuteEditValueSliderFiniched()
    ////{
    ////    ValueSlider = Convert.ToInt32(EditValueSlider / 100);
    ////    IsPopupOpenned = false;
    ////}
}