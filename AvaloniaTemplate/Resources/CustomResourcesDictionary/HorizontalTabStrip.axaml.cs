using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Markup.Xaml.Templates;
using System;
using System.Collections;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class HorizontalTabStrip : TemplatedControl
{
    #region Источник данных раскрывающегося окна
    public static readonly StyledProperty<Panel> PopupContentProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, Panel>(nameof(PopupContent), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных раскрывающегося окна
    /// </summary>
    public Panel PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }
    #endregion

    #region Окно раскрыто
    public static readonly StyledProperty<bool> IsPopupOpennedProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, bool>(nameof(IsPopupOpenned));

    /// <summary>
    /// Окно раскрыто
    /// </summary>
    public bool IsPopupOpenned
    {
        get => GetValue(IsPopupOpennedProperty);
        set => SetValue(IsPopupOpennedProperty, value);
    }
    #endregion

    #region Шаблон представления данных
    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, IDataTemplate?>(nameof(ItemTemplate));

    /// <summary>
    /// Шаблон представления данных
    /// </summary>
    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }
    #endregion

    #region Шаблон панели данных
    public static readonly StyledProperty<ITemplate<Panel?>> ItemsPanelProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, ITemplate<Panel?>>(nameof(ItemsPanel), defaultValue: new ItemsPanelTemplate());

    /// <summary>
    /// Шаблон панели данных
    /// </summary>
    public ITemplate<Panel?> ItemsPanel
    {
        get => GetValue(ItemsPanelProperty);
        set => SetValue(ItemsPanelProperty, value);
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<IList?> SourceItemsProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, IList?>(nameof(SourceItems));

    /// <summary>
    /// Источник данных
    /// </summary>
    public IList? SourceItems
    {
        get => GetValue(SourceItemsProperty);
        set => SetValue(SourceItemsProperty, value);
    }
    #endregion

    #region Выбранный элемент из спсика
    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент из спсика
    /// </summary>
    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    #endregion

    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<HorizontalTabStrip, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    #region Максимальная ширина одного элемента
    public static readonly StyledProperty<double> MaxWidthSingleItemProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, double>(nameof(MaxWidthSingleItem));

    /// <summary>
    /// Максимальная ширина одного элемента
    /// </summary>
    public double MaxWidthSingleItem
    {
        get => GetValue(MaxWidthSingleItemProperty);
        set => SetValue(MaxWidthSingleItemProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var buttonNextLeft = e.NameScope.Find<RepeatButton>("PART_ButtonNextLeft");
        var buttonNextRight = e.NameScope.Find<RepeatButton>("PART_ButtonNextRight");
        var scrollViewer = e.NameScope.Find<ScrollViewer>("PART_Scroll");

        buttonNextLeft.Click += (_, _) => ScrollLeft(MaxWidthSingleItem, scrollViewer);
        buttonNextRight.Click += (_, _) => ScrollRight(MaxWidthSingleItem, scrollViewer);

        //scrollViewer.Offset = 



        //< ScrollViewer x: Name = "PART_Scroll"
    }

    private void ScrollLeft(double delta, ScrollViewer scrollViewer)
    {
        ScrollHorizontal(-delta, scrollViewer);
    }

    private void ScrollRight(double delta, ScrollViewer scrollViewer)
    {
        ScrollHorizontal(delta, scrollViewer);
    }

    private void ScrollHorizontal(double delta, ScrollViewer scrollViewer)
    {
        var max = Math.Max(
            0,
            scrollViewer.Extent.Width -
            scrollViewer.Viewport.Width);

        var x = Math.Clamp(
            scrollViewer.Offset.X + delta,
            0,
            max);

        
        scrollViewer.Offset = new Vector(x, scrollViewer.Offset.Y);
    }
}