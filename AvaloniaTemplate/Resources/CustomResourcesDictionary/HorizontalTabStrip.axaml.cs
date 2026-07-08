using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Collections;
using System.Globalization;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class HorizontalTabStrip : BaseTemplatedControl
{
    static HorizontalTabStrip()
    {
        ItemsSourceProperty.Changed.AddClassHandler<HorizontalTabStrip>((x, _) => x.OnItemsSourceChanged());
    }

    #region Источник данных
    public static readonly StyledProperty<IList?> ItemsSourceProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, IList?>(nameof(ItemsSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public IList? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion

    #region Выбранный элемент
    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент
    /// </summary>
    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    #endregion

    #region Индекс ыыбранного элемента
    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, int>(nameof(SelectedIndex), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Индекс ыыбранного элемента
    /// </summary>
    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
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

    #region Шаблон панели представления данных
    public static readonly StyledProperty<ITemplate<Panel?>> ItemsPanelProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, ITemplate<Panel?>>(nameof(ItemsPanel), defaultValue: new ItemsPanelTemplate());

    /// <summary>
    /// Шаблон панели представления данных
    /// </summary>
    public ITemplate<Panel?> ItemsPanel
    {
        get => GetValue(ItemsPanelProperty);
        set => SetValue(ItemsPanelProperty, value);
    }
    #endregion

    #region Источник данных раскрывающегося окна
    public static readonly StyledProperty<Panel?> ContentPopupProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, Panel?>(nameof(ContentPopup), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных раскрывающегося окна
    /// </summary>
    public Panel? ContentPopup
    {
        get => GetValue(ContentPopupProperty);
        set => SetValue(ContentPopupProperty, value);
    }
    #endregion

    #region Окно раскрыто
    public static readonly StyledProperty<bool> IsPopupOpenProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, bool>(nameof(IsPopupOpen), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Окно раскрыто
    /// </summary>
    public bool IsPopupOpen
    {
        get => GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }
    #endregion

    #region Команда кнопки создания нового элемента
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, ICommand>(nameof(Command));

    /// <summary>
    /// Команда кнопки создания нового элемента
    /// </summary>
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    #endregion

    #region Параметр для команды
    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, object?>(nameof(CommandParameter));

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    #region Размер шага скролла
    public static readonly StyledProperty<double> StepChangeScrollProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, double>(nameof(StepChangeScroll), defaultValue: 50);

    /// <summary>
    /// Размер шага скролла
    /// </summary>
    public double StepChangeScroll
    {
        get => GetValue(StepChangeScrollProperty);
        set => SetValue(StepChangeScrollProperty, value);
    }
    #endregion

    public event Action<object> SelectedItemChange;

    private void OnItemsSourceChanged()
    {
        if (ItemsSource is not { } || CurrTemplateAppliedEventArgs is null)
            return;

        var e = CurrTemplateAppliedEventArgs;
        var scrollViewer = FindPartById<ScrollViewer>(e, "PART_Scroll");

        var buttonNextRight = FindPartById<RepeatButton>(e, "PART_ButtonNextRight");
        buttonNextRight.Click += (_, _)
            => ScrollHorizontal(StepChangeScroll, scrollViewer);

        var buttonNextLeft = FindPartById<RepeatButton>(e, "PART_ButtonNextLeft");
        buttonNextLeft.Click += (_, _)
            => ScrollHorizontal(-StepChangeScroll, scrollViewer);

        ContentPopup ??= CratePopupContent();
        ItemTemplate ??= CrateItemTemplate();
        ItemsPanel = CrateItemsPanel();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (ItemsSource is not { })
            return;

        SetTemplateAppliedEventArgs(e);
        OnItemsSourceChanged();
    }

    private static void ScrollHorizontal(double delta, ScrollViewer scrollViewer)
    {
        var max = Math.Max(0, scrollViewer.Extent.Width - scrollViewer.Viewport.Width);
        var x = Math.Clamp(scrollViewer.Offset.X + delta, 0, max);

        scrollViewer.Offset = new Vector(x, scrollViewer.Offset.Y);
    }

    private IDataTemplate CrateItemTemplate()
    {
        ItemTemplate = new FuncDataTemplate<ModelTable>((item, _) =>
        {
            var tabButton = new ToggleButton()
            {
                MinHeight = 20,
                MinWidth = 70,
                Padding = new(5, 0, 5, 0),
                BorderThickness = new(1, 1, 1, 0),
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Gray,
                CornerRadius = new(3, 3, 0, 0),
                Content = new ContentPresenter()
                {
                    Content = item.Header,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                }
            };
            tabButton.Click += (_, _) => OnSelectedItemChanged(item);
            tabButton.Bind(ToggleButton.IsCheckedProperty, new Binding(nameof(SelectedItem)) { Source = this, Converter = new SelectedItemConverter(item) });
            ToggleGroupHelper.SetGroupIsChecked(tabButton, "TableList");
            return tabButton;
        });
        return ItemTemplate;
    }

    private ITemplate<Panel?> CrateItemsPanel()
    {
        ItemsPanel = new FuncTemplate<Panel?>(() => new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 0
        });
        return ItemsPanel;
    }

    private StackPanel CratePopupContent()
    {
        var viewPopup = new ListBox()
        {
            Margin = new(5),
            BorderThickness = new(2),
            BorderBrush = Brushes.Gray,
            MaxHeight = 200,
        };
        viewPopup.Bind(ItemsControl.ItemsSourceProperty, new Binding("ItemsSource") { Source = this });
        viewPopup.ItemTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            var border = new Border()
            {
                Height = 20,
                Padding = new(3, 3, 3, 0),
                Background = Brushes.Transparent,
                VerticalAlignment = VerticalAlignment.Center,
                Child = new ContentPresenter()
                {
                    Content = item,
                }
            };
            border.DoubleTapped += (_, e) =>
            {
                viewPopup.SelectedIndex = -1;
                ConfirmSelectedItem(item, e);
            };
            return border;
        });

        var panel = new StackPanel()
        {
            Width = 200,
            Spacing = 2,
            Children =
            {
                new TextBlock()
                {
                    Text = "Перейти к...",
                    Margin = new(5, 5, 0, 0),
                    FontWeight = FontWeight.Bold,
                    TextAlignment = TextAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Center
                },
                viewPopup
            }
        };
        return panel;
    }

    private void ConfirmSelectedItem(object item, TappedEventArgs e)
    {
        OnSelectedItemChanged(item);
        IsPopupOpen = false;
    }

    private sealed class SelectedItemConverter(object item) : IValueConverter
    {
        private readonly object _item = item;

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
            => Equals(value, _item);

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => BindingOperations.DoNothing;
    }

    private void OnSelectedItemChanged(object item)
    {
        SelectedItem = item;
        SelectedItemChange?.Invoke(item);
    }
}