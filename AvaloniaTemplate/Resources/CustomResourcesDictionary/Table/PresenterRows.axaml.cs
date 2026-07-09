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

public class PresenterRows : BaseTemplatedControl
{
    private static readonly IBrush SeparatorBrush = Brushes.WhiteSmoke;
    private readonly TranslateTransform transform = new();
    private static readonly double WidthSeparator = 1;
    private static bool IsMousePressed;
    private ContentPresenter presenter;

    static PresenterRows()
    {
        ItemsSourceProperty.Changed.AddClassHandler<PresenterRows>((x, _) => x.RebuildContent());
        PositionXProperty.Changed.AddClassHandler<PresenterRows>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterRows>((x, _) => x.UpdateTransform());
    }

    #region Событие изменения текущего элемента
    /// <summary>
    /// Событие изменения текущего элемента
    /// T: Элемент
    /// </summary>
    public event Action<PointerPressedEventArgs, ModelRow> SelectedItemChanged;
    #endregion

    #region Событие устанвоки фокуса элемента
    /// <summary>
    /// Событие устанвоки фокуса элемента
    /// T: Элемент
    /// </summary>
    public event Action<ModelRow> SetFocusItem;
    #endregion

    #region Событие снятия фокуса элемента
    /// <summary>
    /// Событие снятия фокуса элемента
    /// T: Элемент
    /// </summary>
    public event Action<ModelRow> ResetFocusItem;
    #endregion

    #region Событие перемещения мыши по панели
    /// <summary>
    /// Событие перемещения мыши по панели
    /// T: Элемент
    /// </summary>
    public event Action<Control?, PointerEventArgs> PointerMovedEventChange;
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
    private sealed class RowDefinitionHeightConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var gridLength = new GridLength();
            if (value is double height)
                gridLength = new GridLength(height, GridUnitType.Pixel);

            return gridLength;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => BindingOperations.DoNothing;
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<ObservableCollection<ModelRow>> ItemsSourceProperty =
        AvaloniaProperty.Register<PresenterRows, ObservableCollection<ModelRow>>(nameof(ItemsSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public ObservableCollection<ModelRow> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion

    #region Выбранный элемент
    public static readonly StyledProperty<ModelRow> SelectedItemProperty =
        AvaloniaProperty.Register<PresenterRows, ModelRow>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент
    /// </summary>
    public ModelRow SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    #endregion

    #region Контент
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<PresenterRows, object?>(nameof(Content));

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
        AvaloniaProperty.Register<PresenterRows, double>(nameof(PositionX));

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
        AvaloniaProperty.Register<PresenterRows, double>(nameof(PositionY));

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
        var rows = ItemsSource.Select(x =>
        {
            var row = new RowDefinition();
            row.Bind(
                RowDefinition.HeightProperty,
                new Binding(nameof(x.Height))
                {
                    Source = x,
                    Mode = BindingMode.TwoWay,
                    Converter = new RowDefinitionHeightConverter()
                });
            return row;
        })?.ToList();

        rows.Add(new RowDefinition(5, GridUnitType.Pixel));
        var grid = new Grid() { RowDefinitions = [.. rows] };
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
            border.PointerPressed += (_, e) => OnSelectedItemChanged(e, item);
            border.PointerEntered += (_, _) => SetFocusItem?.Invoke(item);
            border.PointerExited += (_, _) => ResetFocusItem?.Invoke(item);

            Grid.SetRow(border, item.Index);
            Grid.SetRow(splitter, item.Index);
            Grid.SetRow(separator, item.Index);
            grid.Children.Add(border);
            grid.Children.Add(splitter);
            grid.Children.Add(separator);
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
        transform.Y = -PositionY;
        transform.X = PositionX;
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
            ResizeDirection = GridResizeDirection.Rows,
            VerticalAlignment = VerticalAlignment.Bottom,
            MinWidth = 0,
            Height = 5,
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
            Height = WidthSeparator,
            Margin = new(0, 0, 0, 0),
            IsHitTestVisible = false,
            VerticalAlignment = VerticalAlignment.Bottom
        };
    }
    #endregion

    #region Поулчить элемент управления
    /// <summary>
    /// Поулчить элемент управления
    /// </summary>
    /// <param name="Item"></param>
    /// <returns></returns>
    private static TextBlock GetItemControl(ModelRow Item)
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
    private void OnSelectedItemChanged(PointerPressedEventArgs e, ModelRow item)
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