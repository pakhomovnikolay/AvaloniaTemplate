using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterColumns : BaseTemplatedControl
{
    private static bool IsMousePressed;
    private readonly TranslateTransform transform = new();
    private static readonly IBrush SeparatorBrush = Brushes.WhiteSmoke;
    private static readonly double WidthSeparator = 1;
    private ContentPresenter presenter;

    static PresenterColumns()
    {
        ItemsSourceProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.RebuildContent());
        PositionXProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.UpdateTransform());
    }

    #region Событие изменения текущего элемента
    /// <summary>
    /// Событие изменения текущего элемента
    /// </summary>
    public event Action<PointerPressedEventArgs, ModelColumn> SelectedItemChanged;
    #endregion

    #region Событие устанвоки фокуса элемента
    /// <summary>
    /// Событие устанвоки фокуса элемента
    /// </summary>
    public event Action<ModelColumn> SetFocusItem;
    #endregion

    #region Событие снятия фокуса элемента
    /// <summary>
    /// Событие снятия фокуса элемента
    /// </summary>
    public event Action<ModelColumn> ResetFocusItem;
    #endregion

    #region Событие перемещения мыши по панели
    /// <summary>
    /// Событие перемещения мыши по панели
    /// </summary>
    public event Action<Control?, PointerEventArgs> PointerMovedEventChange;
    #endregion

    #region Событие начала изменения ширины
    /// <summary>
    /// Событие начала изменения ширины
    /// </summary>
    public event Action<ModelColumn> DragStartedEvent;
    #endregion

    #region Событие изменения ширины
    /// <summary>
    /// Событие изменения ширины
    /// </summary>
    public event Action<ModelColumn, double> WidthChangeEvent;
    #endregion

    #region Событие завершения изменения ширины
    /// <summary>
    /// Событие завершения изменения ширины
    /// </summary>
    public event Action<ModelColumn> DragCompletedEvent;
    #endregion

    #region Событие необходимости установки ширины по содержимому
    /// <summary>
    /// Событие необходимости установки ширины по содержимому
    /// </summary>
    public event Action<ModelColumn> SizeToContentEvent;
    #endregion

    #region Конвертер заднего фона из строки
    /// <summary>
    /// Конвертер заднего фона из строки
    /// </summary>
    private sealed class BackgroundConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            IBrush color = Brushes.Transparent;
            if (value is string brush)
                color = Helper.GetColor(brush);

            return color;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => BindingOperations.DoNothing;
    }
    #endregion

    #region Конвертер ширины в GridLength
    /// <summary>
    /// Конвертер ширины в GridLength
    /// </summary>
    private sealed class ColumnDefinitionWidthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var gridLength = new GridLength();
            if (value is double width)
                gridLength = new GridLength(width, GridUnitType.Pixel);

            return gridLength;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is GridLength length)
                return length.Value;

            return BindingOperations.DoNothing;
        }
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<ObservableCollection<ModelColumn>> ItemsSourceProperty =
        AvaloniaProperty.Register<PresenterColumns, ObservableCollection<ModelColumn>>(nameof(ItemsSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public ObservableCollection<ModelColumn> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion

    #region Выбранный элемент
    public static readonly StyledProperty<ModelColumn> SelectedItemProperty =
        AvaloniaProperty.Register<PresenterColumns, ModelColumn>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент
    /// </summary>
    public ModelColumn SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    #endregion

    #region Контент
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<PresenterColumns, object?>(nameof(Content));

    /// <summary>
    /// Контент
    /// </summary>
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    #endregion

    #region Положение по горизонтали
    public static readonly StyledProperty<double> PositionXProperty =
        AvaloniaProperty.Register<PresenterColumns, double>(nameof(PositionX));

    /// <summary>
    /// Положение по горизонтали
    /// </summary>
    public double PositionX
    {
        get => GetValue(PositionXProperty);
        set => SetValue(PositionXProperty, value);
    }
    #endregion

    #region Положение по вертикали
    public static readonly StyledProperty<double> PositionYProperty =
        AvaloniaProperty.Register<PresenterColumns, double>(nameof(PositionY));

    /// <summary>
    /// Положение по вертикали
    /// </summary>
    public double PositionY
    {
        get => GetValue(PositionYProperty);
        set => SetValue(PositionYProperty, value);
    }
    #endregion

    #region Пересобрать контент
    /// <summary>
    /// Пересобрать контент
    /// </summary>
    private void RebuildContent()
    {
        Content = InitializeContent();
    }
    #endregion

    #region Инициализация контента
    /// <summary>
    /// Инициализация контента
    /// </summary>
    /// <returns></returns>
    private Grid InitializeContent()
    {
        var columns = ItemsSource.Select(x =>
        {
            var column = new ColumnDefinition();
            column.Bind(
                ColumnDefinition.WidthProperty,
                new Binding(nameof(x.Width))
                {
                    Source = x,
                    Mode = BindingMode.TwoWay,
                    Converter = new ColumnDefinitionWidthConverter()
                });
            return column;
        })?.ToList();
        columns.Add(new ColumnDefinition(5, GridUnitType.Pixel));

        var grid = new Grid() { ColumnDefinitions = [.. columns] };
        foreach (var item in ItemsSource)
        {
            var splitter = GetGridSplitter();
            var separator = GetViewwSplitter();
            var border = new Border()
            {
                DataContext = item,
                Child = GetItemControl(item)
            };
            border.Bind(Border.BackgroundProperty, new Binding("CellStyle.Background") { Converter = new BackgroundConverter() });
            border.Bind(IsVisibleProperty, new Binding(nameof(item.IsVisible)));
            border.PointerPressed += (_, e) => OnSelectedItemChanged(e, item);
            border.PointerEntered += (_, _) => SetFocusItem?.Invoke(item);
            border.PointerExited += (_, _) => ResetFocusItem?.Invoke(item);

            Grid.SetColumn(border, item.Index);
            Grid.SetColumn(splitter, item.Index);
            Grid.SetColumn(separator, item.Index);
            grid.Children.Add(border);
            grid.Children.Add(splitter);
            grid.Children.Add(separator);

            splitter.DragStarted += (_, _) => DragStartedEvent?.Invoke(item);
            splitter.DragDelta += (_, e) => WidthChangeEvent?.Invoke(item, e.Vector.X);
            splitter.DragCompleted += (_, e) => DragCompletedEvent?.Invoke(item);
            splitter.DoubleTapped += (_, e) => SizeToContentEvent?.Invoke(item);
        }
        return grid;
    }
    #endregion

    #region Обновить положение
    /// <summary>
    /// Обновить положение
    /// </summary>
    private void UpdateTransform()
    {
        transform.Y = PositionY;
        transform.X = -PositionX;
    }
    #endregion

    #region Получить разделитель
    /// <summary>
    /// Получить разделитель
    /// </summary>
    /// <returns></returns>
    private static GridSplitter GetGridSplitter()
    {
        return new GridSplitter
        {
            Background = Brushes.Transparent,
            BorderThickness = new(0),
            ResizeDirection = GridResizeDirection.Columns,
            HorizontalAlignment = HorizontalAlignment.Right,
            MinWidth = 0,
            Width = 10,
            Margin = new(0, 0, 0, 0)
        };
    }
    #endregion

    #region Получить визуальный разделитель
    /// <summary>
    /// Получить визуальный разделитель
    /// </summary>
    /// <returns></returns>
    private static Rectangle GetViewwSplitter()
    {
        return new Rectangle
        {
            Fill = SeparatorBrush,
            Width = WidthSeparator,
            Margin = new(0, 0, 0, 0),
            IsHitTestVisible = false,
            HorizontalAlignment = HorizontalAlignment.Right
        };
    }
    #endregion

    #region Поулчить элемент управления
    /// <summary>
    /// Поулчить элемент управления
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    private static TextBlock GetItemControl(ModelColumn Item)
    {
        var control = new TextBlock()
        {
            DataContext = Item,
            Text = Item.Header,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
            FontFamily = Item.CellStyle.FontFamily,
            FontSize = Item.CellStyle.FontSize,
            FontWeight = Item.CellStyle.IsBold ? FontWeight.Bold : FontWeight.Normal
        };
        control.Bind(TextBlock.ForegroundProperty, new Binding("CellStyle.Foreground"));
        return control;
    }
    #endregion

    #region Обработка смены выбора элемента
    /// <summary>
    /// Обработка смены выбора элемента
    /// </summary>
    /// <param name="e"></param>
    /// <param name="item"></param>
    private void OnSelectedItemChanged(PointerPressedEventArgs e, ModelColumn item)
    {
        if (!e.Properties.IsLeftButtonPressed || item is not { })
            return;

        SelectedItemChanged?.Invoke(e, item);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (ItemsSource is not { } || ItemsSource.Count <= 0)
            return;

        presenter = FindPartById<ContentPresenter>(e, "PART_ContentPresenter");
        presenter.RenderTransform = transform;
        Content ??= InitializeContent();
    }
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (!e.Properties.IsLeftButtonPressed)
            return;

        IsMousePressed = true;
    }
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        IsMousePressed = false;
    }
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!IsMousePressed)
            return;

        PointerMovedEventChange?.Invoke(presenter, e);
    }
}