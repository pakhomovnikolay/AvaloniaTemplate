using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.LayoutControls;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Globalization;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterColumns : BaseTemplatedControl
{
    private bool IsMousePressed;
    private readonly TranslateTransform transform = new();
    private readonly IBrush SeparatorBrush = Brushes.WhiteSmoke;
    private readonly double WidthSeparator = 1;
    private SpreadsheetPanel presenter;

    static PresenterColumns()
    {
        ModelProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.RebuildContent());
        PositionXProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.UpdateTransform());
        ScaleProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.RebuildContent());
    }

    #region Активная область
    public static readonly StyledProperty<LayoutFrame> FrameProperty =
        AvaloniaProperty.Register<PresenterColumns, LayoutFrame>(nameof(Frame));

    /// <summary>
    /// Активная область
    /// </summary>
    public LayoutFrame Frame
    {
        get => GetValue(FrameProperty);
        set => SetValue(FrameProperty, value);
    }
    #endregion

    #region Конвертер заднего фона из строки
    /// <summary>
    /// Конвертер заднего фона из строки
    /// </summary>
    private sealed class SolidColorBrushConverter : IValueConverter
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

    #region Конвертер толщины шрифта
    /// <summary>
    /// Конвертер толщины шрифта
    /// </summary>
    private sealed class FontWeightConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var fontWeight = FontWeight.Normal;
            if (value is bool isBold && isBold)
                fontWeight = FontWeight.Bold;

            return fontWeight;
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
        public double scale = 1;

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (scale < 0.1)
                scale = 0.1;


            var gridLength = new GridLength();
            if (value is double width)
                gridLength = new GridLength(width * scale, GridUnitType.Pixel);

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

    #region Модель данных
    public static readonly StyledProperty<ModelTable> ModelProperty =
        AvaloniaProperty.Register<PresenterColumns, ModelTable>(nameof(Model));

    /// <summary>
    /// Модель данных
    /// </summary>
    public ModelTable Model
    {
        get => GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
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

    #region Масштаб
    public static readonly StyledProperty<double> ScaleProperty =
        AvaloniaProperty.Register<PresenterColumns, double>(nameof(Scale), defaultValue: 1);

    /// <summary>
    /// Масштаб
    /// </summary>
    public double Scale
    {
        get => GetValue(ScaleProperty);
        set => SetValue(ScaleProperty, value);
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

    #region Пересобрать контент
    /// <summary>
    /// Пересобрать контент
    /// </summary>
    private void RebuildContent()
    {
        presenter = InitializeContent();
        presenter.RenderTransform = transform;
        Content = presenter;

        //presenter = FindPartById<ContentPresenter>(e, "PART_ContentPresenter");
        //presenter.RenderTransform = transform;

        //Content ??= InitializeContent();
        //Content = Model.Columns;
    }
    #endregion

    #region Инициализация контента
    /// <summary>
    /// Инициализация контента
    /// </summary>
    /// <returns></returns>
    private SpreadsheetPanel InitializeContent()
    {
        var panel = new SpreadsheetPanel();
        panel.Bind(SpreadsheetPanel.ZoomProperty, new Binding(nameof(Scale)) { Source = this });
        foreach (var column in Model?.Columns)
        {
            panel.Children.Add(new ContentPresenter()
            {
                Content = column,
                ContentTemplate = new FuncDataTemplate<ModelColumn>((item, _) => GetItemControl(item))
                //{
                //    return new Panel() { Children = { GetItemControl(item)/*, GetViewwSplitter() */} };
                //})
            });
        }

        return panel;



        //return new()
        //{
        //    Content = Model.Columns
        //};




        //var panel = new SpreadsheetPanel();
        //panel.Bind(SpreadsheetPanel.ZoomProperty, new Binding(nameof(Scale)) { Source = this });

        //foreach (var column in Model?.Columns)
        //{
        //    panel.Children.Add(new ContentPresenter()
        //    {
        //        Content = column,
        //        ContentTemplate = new FuncDataTemplate<ModelColumn>((item, _) =>
        //        {
        //            return new Panel() { Children = { GetItemControl(item), GetViewwSplitter() } };
        //        })
        //    });
        //}
        //return panel;
    }
    #endregion





    //comboBox.ItemTemplate = new FuncDataTemplate<FontFamily>((item, _) =>
    //{
    //    var fontFamily = item ?? FontFamilyHelper.FontDefault;
    //    return CreateBorderItemTemplate(fontFamily.Name, fontFamily, fontFamily);
    //});


    //var panel = new SpreadsheetPanel();
    //panel.Bind(SpreadsheetPanel.ZoomProperty, new Binding(nameof(Scale)) { Source = this });
    //foreach (var column in Model?.Columns)
    //{
    //    panel.Children.Add(new ContentPresenter()
    //    {
    //        DataContext = column,
    //        Content = GetItemControl(column)
    //    });
    //}
    //return panel;

    //var columns = Model?.Columns.Select(x =>
    //{
    //    var column = new ColumnDefinition();
    //    column.Bind(
    //        ColumnDefinition.WidthProperty,
    //        new Binding(nameof(x.Width))
    //        {
    //            Source = x,
    //            Mode = BindingMode.TwoWay,
    //            Converter = new ColumnDefinitionWidthConverter() { scale = Scale }
    //        });
    //    return column;
    //})?.ToList();
    //columns.Add(new ColumnDefinition(5, GridUnitType.Pixel));

    ////var scaleTransform = new ScaleTransform();
    ////scaleTransform.Bind(ScaleTransform.ScaleXProperty, new Binding("Scale") { Source = this });
    //var grid = new Grid()
    //{
    //    ColumnDefinitions = [.. columns],
    //    //RenderTransformOrigin = new(1, 1, RelativeUnit.Absolute),
    //    //RenderTransform = scaleTransform
    //};

    //foreach (var item in Model?.Columns)
    //{

    //    var splitter = GetGridSplitter();
    //    var separator = GetViewwSplitter();
    //    var border = new Border()
    //    {
    //        DataContext = item,
    //        Child = GetItemControl(item),
    //        VerticalAlignment = VerticalAlignment.Stretch
    //    };
    //    border.Bind(Border.BackgroundProperty, new Binding("CellStyle.Background") { Converter = new BackgroundConverter() });
    //    border.Bind(IsVisibleProperty, new Binding(nameof(item.IsVisible)));
    //    border.PointerPressed += (_, e) => Model?.SetSelectedColumn(e, item);
    //    border.PointerEntered += (_, _) => Model?.SetFocusColumn(item);
    //    border.PointerExited += (_, _) => Model?.ResetFocusColumn(item);

    //    Grid.SetColumn(border, item.Index);
    //    Grid.SetColumn(splitter, item.Index);
    //    Grid.SetColumn(separator, item.Index);
    //    grid.Children.Add(border);
    //    grid.Children.Add(splitter);
    //    grid.Children.Add(separator);

    //    splitter.DragStarted += (_, _) => Model?.ColumnDragStartedChange?.Invoke(Orientation.Vertical, item.PositionX, item.Right);
    //    splitter.DragDelta += (_, e) => OnDragDelta(e.Vector.X, splitter.Width, item);
    //    splitter.DragCompleted += (_, e) => Model?.ColumnDragCompletedChange?.Invoke(Orientation.Vertical, item);
    //    splitter.DoubleTapped += (_, e) => Model?.ColumnSplitterDoubleTappedChange?.Invoke(item);
    //}
    //return grid;




    #region Изменение размера колонки
    /// <summary>
    /// Изменение размера колонки
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="item"></param>
    private void OnDragDelta(double delta, double maxWidth, ModelColumn item)
    {
        var minDelta = 0.5;
        if (Math.Abs(delta) < minDelta)
            return;

        if (item.Width <= maxWidth)
            item.Right = item.PositionX + item.Width;
        else
            item.Right += delta;

        Model?.ColumnDragStartedChange?.Invoke(Orientation.Vertical, item.PositionX, item.Right);
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
    private Rectangle GetViewwSplitter()
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
    private Panel GetItemControl(ModelColumn item)
    {
        var viewPanel = new TextBlock()
        {
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
        viewPanel.Bind(TextBlock.TextProperty, new Binding("Header"));
        viewPanel.Bind(TextBlock.FontFamilyProperty, new Binding("CellStyle.FontFamily"));
        viewPanel.Bind(TextBlock.FontSizeProperty, new Binding("CellStyle.FontSize"));
        viewPanel.Bind(TextBlock.FontWeightProperty, new Binding("CellStyle.IsBold") { Converter = new FontWeightConverter() });
        viewPanel.Bind(TextBlock.ForegroundProperty, new Binding("CellStyle.Foreground") { Converter = new SolidColorBrushConverter() });

        var border = new Border()
        {
            DataContext = item,
            VerticalAlignment = VerticalAlignment.Stretch,
            Child = viewPanel
        };
        border.Bind(Border.BackgroundProperty, new Binding("CellStyle.Background") { Converter = new SolidColorBrushConverter() });
        border.Bind(IsVisibleProperty, new Binding(nameof(item.IsVisible)));
        border.PointerEntered += (_, _) => Model?.SetFocusColumn(item);
        border.PointerExited += (_, _) => Model?.ResetFocusColumn(item);
        border.PointerPressed += (_, e) => Model?.SetSelectedColumn(e, item);

        var splitter = GetGridSplitter();
        splitter.DragStarted += (_, _) => Model?.ColumnDragStartedChange?.Invoke(Orientation.Vertical, item.PositionX, item.Right);
        splitter.DragDelta += (_, e) => OnDragDelta(e.Vector.X, splitter.Width, item);
        splitter.DragCompleted += (_, e) => Model?.ColumnDragCompletedChange?.Invoke(Orientation.Vertical, item);
        splitter.DoubleTapped += (_, e) => Model?.ColumnSplitterDoubleTappedChange?.Invoke(item);
        return new() { Children = { border, splitter, GetViewwSplitter() } };



        //var control = new TextBlock()
        //{
        //    DataContext = Item,
        //    Text = Item.Header,
        //    HorizontalAlignment = HorizontalAlignment.Center,
        //    VerticalAlignment = VerticalAlignment.Center,
        //    FontFamily = Item.CellStyle.FontFamily,
        //    FontSize = Item.CellStyle.FontSize,
        //    FontWeight = Item.CellStyle.IsBold ? FontWeight.Bold : FontWeight.Normal
        //};
        //control.Bind(TextBlock.ForegroundProperty, new Binding("CellStyle.Foreground"));

        //return control;



        //var splitter = GetGridSplitter();
        //var separator = GetViewwSplitter();
        //var border = new Border()
        //{
        //    DataContext = item,
        //    Child = GetItemControl(item),
        //    VerticalAlignment = VerticalAlignment.Stretch
        //};
        //border.Bind(Border.BackgroundProperty, new Binding("CellStyle.Background") { Converter = new BackgroundConverter() });
        //border.Bind(IsVisibleProperty, new Binding(nameof(item.IsVisible)));
        //border.PointerPressed += (_, e) => Model?.SetSelectedColumn(e, item);
        //border.PointerEntered += (_, _) => Model?.SetFocusColumn(item);
        //border.PointerExited += (_, _) => Model?.ResetFocusColumn(item);

        //Grid.SetColumn(border, item.Index);
        //Grid.SetColumn(splitter, item.Index);
        //Grid.SetColumn(separator, item.Index);
        //grid.Children.Add(border);
        //grid.Children.Add(splitter);
        //grid.Children.Add(separator);

        //splitter.DragStarted += (_, _) => Model?.ColumnDragStartedChange?.Invoke(Orientation.Vertical, item.PositionX, item.Right);
        //splitter.DragDelta += (_, e) => OnDragDelta(e.Vector.X, splitter.Width, item);
        //splitter.DragCompleted += (_, e) => Model?.ColumnDragCompletedChange?.Invoke(Orientation.Vertical, item);
        //splitter.DoubleTapped += (_, e) => Model?.ColumnSplitterDoubleTappedChange?.Invoke(item);
    }
    #endregion


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (Model is not { } || Model.Columns.Count <= 0)
            return;

        RebuildContent();

        //presenter = FindPartById<ContentPresenter>(e, "PART_ContentPresenter");
        //presenter.RenderTransform = transform;

        //Content ??= InitializeContent();
        //Content = Model.Columns;
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

        Model?.ColumnsPointerMovedEvent(presenter, e);
    }
}