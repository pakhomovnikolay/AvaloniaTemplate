using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Infrastructures.Helpers;
using System;
using System.Collections;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class HorizontalTabStrip : TemplatedControl
{
    static HorizontalTabStrip()
    {
        SelectedItemProperty.Changed.AddClassHandler<HorizontalTabStrip>((x, _) => x.SynchronizeIndexItem());
        SelectedIndexProperty.Changed.AddClassHandler<HorizontalTabStrip>((x, _) => x.SynchronizeItem());
    }

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

    #region Команда создания нового элемента
    public static readonly StyledProperty<ICommand> CommandCreateItemProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, ICommand>(nameof(CommandCreateItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Команда создания нового элемента
    /// </summary>
    public ICommand CommandCreateItem
    {
        get => GetValue(CommandCreateItemProperty);
        set => SetValue(CommandCreateItemProperty, value);
    }
    #endregion

    #region Команда выбора элемента
    public static readonly StyledProperty<ICommand> CommandSelectedItemProperty =
        AvaloniaProperty.Register<HorizontalTabStrip, ICommand>(nameof(CommandSelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Команда выбора элемента
    /// </summary>
    public ICommand CommandSelectedItem
    {
        get => GetValue(CommandSelectedItemProperty);
        set => SetValue(CommandSelectedItemProperty, value);
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
        if (ItemsSource is not { })
            return;

        var scrollViewer = e.NameScope.Find<ScrollViewer>("PART_Scroll");

        var buttonNextLeft = e.NameScope.Find<RepeatButton>("PART_ButtonNextLeft");
        buttonNextLeft.Command = Command_SelectNextItemLeft;
        buttonNextLeft.CommandParameter = scrollViewer;

        var buttonNextRight = e.NameScope.Find<RepeatButton>("PART_ButtonNextRight");
        buttonNextRight.Command = Command_SelectNextItemRight;
        buttonNextRight.CommandParameter = scrollViewer;

        PopupContent ??= CratePopupContent();
        ItemTemplate ??= CrateItemTemplate();
        ItemsPanel ??= CrateItemsPanel();
    }

    private static void ScrollLeft(double delta, ScrollViewer scrollViewer)
        => ScrollHorizontal(-delta, scrollViewer);

    private static void ScrollRight(double delta, ScrollViewer scrollViewer)
        => ScrollHorizontal(delta, scrollViewer);

    private static void ScrollHorizontal(double delta, ScrollViewer scrollViewer)
    {
        var max = Math.Max(0, scrollViewer.Extent.Width - scrollViewer.Viewport.Width);
        var x = Math.Clamp(scrollViewer.Offset.X + delta, 0, max);

        scrollViewer.Offset = new Vector(x, scrollViewer.Offset.Y);
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


    private static ICommand command_SelectNextItemLeft;
    private ICommand Command_SelectNextItemLeft
        => command_SelectNextItemLeft ??= new RelayCommand(ExecuteSelectNextItemLeft);

    private void ExecuteSelectNextItemLeft(object p)
    {
        if (p is not { } || p is not ScrollViewer scrollViewer)
            return;

        ScrollLeft(MaxWidthSingleItem, scrollViewer);
    }

    private static ICommand command_SelectNextItemRight;
    private ICommand Command_SelectNextItemRight
        => command_SelectNextItemRight ??= new RelayCommand(ExecuteSelectNextItemRight);

    private void ExecuteSelectNextItemRight(object p)
    {
        if (p is not { } || p is not ScrollViewer scrollViewer)
            return;

        ScrollRight(MaxWidthSingleItem, scrollViewer);
    }

    private void ConfirmSelectedItem(object item, TappedEventArgs e)
    {
        SelectedItem = item;
        IsPopupOpenned = false;
        CommandSelectedItem?.Execute(item);
    }

    private void SynchronizeIndexItem()
    {
        var index = ItemsSource.IndexOf(SelectedItem);
        if (SelectedIndex == index)
            return;

        SelectedIndex = index;
    }

    private void SynchronizeItem()
    {
        var item = ItemsSource[SelectedIndex];
        if (item.Equals(SelectedItem))
            return;

        SelectedItem = item;
    }

    private IDataTemplate CrateItemTemplate()
    {
        ItemTemplate = new FuncDataTemplate<object>((item, _) =>
        {
            var tabButton = new ToggleButton()
            {
                Height = 20,
                MinWidth = 70,
                Padding = new(5, 0, 5, 0),
                BorderThickness = new(1, 1, 1, 0),
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Gray,
                CornerRadius = new(3, 3, 0, 0),
                Content = new ContentPresenter()
                {
                    Content = item,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                }
            };
            ToggleGroupHelper.SetGroupClick(tabButton, "TableList");
            return tabButton;
        });
        return ItemTemplate;
    }

    private ITemplate<Panel?> CrateItemsPanel()
    {
        ItemsPanel = new ItemsPanelTemplate()
        {
            Content = new StackPanel()
            {
                Orientation = Orientation.Horizontal,
                Spacing = 1
            }
        };
        return ItemsPanel;
    }
}