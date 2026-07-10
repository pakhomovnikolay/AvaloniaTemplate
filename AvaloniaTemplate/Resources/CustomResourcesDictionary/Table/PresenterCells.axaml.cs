using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.LayoutControls;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Globalization;
using System.Linq;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterCells : BaseTemplatedControl
{
    private static bool IsMousePressed;
    private readonly TranslateTransform transform = new();
    private ContentPresenter presenter;

    static PresenterCells()
    {
        ItemsSourceProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.RebuildContent());
        PositionXProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.UpdateTransform());
    }

    #region Событие изменения текущего элемента
    /// <summary>
    /// Событие изменения текущего элемента
    /// </summary>
    public event Action<PointerPressedEventArgs, ModelCell> SelectedItemChanged;
    #endregion



    #region Событие перемещения мыши по панели
    /// <summary>
    /// Событие перемещения мыши по панели
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
            => BindingOperations.DoNothing;
    }
    #endregion

    #region Конвертер высоты в GridLength
    /// <summary>
    /// Конвертер высоты в GridLength
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
    public static readonly StyledProperty<LayoutDragArea> DragAreaProperty =
        AvaloniaProperty.Register<PresenterCells, LayoutDragArea>(nameof(DragArea));

    /// <summary>
    /// Источник данных
    /// </summary>
    public LayoutDragArea DragArea
    {
        get => GetValue(DragAreaProperty);
        set => SetValue(DragAreaProperty, value);
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<ModelTable> ItemsSourceProperty =
        AvaloniaProperty.Register<PresenterCells, ModelTable>(nameof(ItemsSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public ModelTable ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion

    #region Выбранный элемент
    public static readonly StyledProperty<ModelCell> SelectedItemProperty =
        AvaloniaProperty.Register<PresenterCells, ModelCell>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент
    /// </summary>
    public ModelCell SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    #endregion

    #region Контент
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<PresenterCells, object?>(nameof(Content));

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
        AvaloniaProperty.Register<PresenterCells, double>(nameof(PositionX));

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
        AvaloniaProperty.Register<PresenterCells, double>(nameof(PositionY));

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
        var columns = ItemsSource.Columns.Select(x =>
        {
            var column = new ColumnDefinition();
            column.Bind(
                ColumnDefinition.WidthProperty,
                new Binding(nameof(x.WidthResult))
                {
                    Source = x,
                    Mode = BindingMode.TwoWay,
                    Converter = new ColumnDefinitionWidthConverter()
                });
            return column;
        })?.ToList();
        columns.Add(new ColumnDefinition(5, GridUnitType.Pixel));

        var rows = ItemsSource.Rows.Select(x =>
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

        var grid = new Grid()
        {
            ColumnDefinitions = [.. columns],
            RowDefinitions = [.. rows]
        };

        for (var i = 0; i < ItemsSource.Rows.Count; i++)
        {
            var row = ItemsSource.Rows[i];
            for (var j = 0; j < row.Cells.Count; j++)
            {
                var item = row.Cells[j];
                var border = new Border()
                {
                    BorderBrush = Helper.GetColor(item.CellStyle.BorderBrush),
                    BorderThickness = new(0, 0, 1, 1),
                    DataContext = item
                };
                border.Bind(Border.BackgroundProperty, new Binding("CellStyle.Background") { Converter = new BackgroundConverter() });
                border.Bind(IsVisibleProperty, new Binding(nameof(item.IsVisible)));
                border.PointerPressed += (_, e) => OnSelectedItemChanged(e, item);

                Grid.SetColumn(border, item.ColumnIndex);
                Grid.SetRow(border, item.RowIndex);
                grid.Children.Add(border);
            }
        }

        grid.RowDefinitions.Add(new RowDefinition(5, GridUnitType.Pixel));
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
        transform.X = -PositionX;
    }
    #endregion

    #region Обработка смены выбора элемента
    /// <summary>
    /// Обработка смены выбора элемента
    /// </summary>
    /// <param name="e"></param>
    /// <param name="item"></param>
    private void OnSelectedItemChanged(PointerPressedEventArgs e, ModelCell item)
    {
        if (!e.Properties.IsLeftButtonPressed || item is not { })
            return;

        SelectedItemChanged?.Invoke(e, item);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (ItemsSource is not { } || ItemsSource.Rows.Count <= 0 || ItemsSource.Rows.FirstOrDefault(r => r.Cells.Count > 0) is not { })
            return;

        var panel = FindPartById<Panel>(e, "PART_RootPanel");
        panel.Children.Add(DragArea);

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