using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Table.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterCells : BaseTemplatedControl
{
    private readonly TranslateTransform transform = new();
    private static readonly IBrush SeparatorBrush = Brushes.WhiteSmoke;
    private static readonly double WidthSeparator = 1;
    private ContentPresenter presenter;

    static PresenterCells()
    {
        ItemsSourceProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.RebuildContent());
        PositionXProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.UpdateTransform());
    }

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

    #region Content
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<PresenterCells, object?>(nameof(Content));

    /// <summary>
    /// Content
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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (ItemsSource is not { } || ItemsSource.Rows.Count <= 0 || ItemsSource.Rows.FirstOrDefault(r => r.Cells.Count > 0) is not { })
            return;

        presenter = FindPartById<ContentPresenter>(e, "PART_ContentPresenter");
        presenter.RenderTransform = transform;
        Content ??= InitializeContent();
    }

    private Grid InitializeContent()
    {
        var grid = new Grid()
        {
            RowDefinitions = [],
            ColumnDefinitions = []
        };

        foreach (var item in ItemsSource.Columns)
        {
            var columnDefinition = new ColumnDefinition();
            columnDefinition.Bind(
                ColumnDefinition.WidthProperty,
                new Binding(nameof(item.Width))
                {
                    Source = item,
                    Mode = BindingMode.TwoWay,
                    Converter = new ColumnDefinitionWidthConverter()
                });

            grid.ColumnDefinitions.Add(columnDefinition);
        }
        grid.ColumnDefinitions.Add(new ColumnDefinition(0, GridUnitType.Star));

        foreach (var item in ItemsSource.Rows)
        {
            var rowDefinition = new RowDefinition();
            rowDefinition.Bind(
                RowDefinition.HeightProperty,
                new Binding(nameof(item.Height))
                {
                    Source = item,
                    Mode = BindingMode.TwoWay,
                    Converter = new RowDefinitionWidthConverter()
                });

            grid.RowDefinitions.Add(rowDefinition);
        }
        grid.RowDefinitions.Add(new RowDefinition(0, GridUnitType.Star));

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

                Grid.SetColumn(border, item.ColumnIndex);
                Grid.SetRow(border, item.RowIndex);
                grid.Children.Add(border);
            }
        }

        //foreach (var item in ItemsSource)
        //{
        //    var rowDefinition = new RowDefinition();
        //    var splitter = GetGridSplitter();
        //    var separator = GetViewwSplitter();
        //    var border = new Border()
        //    {
        //        DataContext = item,
        //        Child = GetItemControl(item)
        //    };

        //    border.Bind(Border.BackgroundProperty, new Binding("CellStyle.Background") { Converter = new BackgroundConverter() });
        //    rowDefinition.Bind(RowDefinition.HeightProperty,
        //        new Binding("Height")
        //        {
        //            Source = item,
        //            Mode = BindingMode.TwoWay,
        //            Converter = new ColumnDefinitionWidthConverter()
        //        });


        //    //tabButton.Bind(ToggleButton.IsCheckedProperty, new Binding(nameof(SelectedItem)) { Source = this, Converter = new SelectedItemConverter(item) });






        //    Grid.SetRow(border, item.Index);
        //    Grid.SetRow(splitter, item.Index);
        //    Grid.SetRow(separator, item.Index);

        //    grid.RowDefinitions.Add(rowDefinition);
        //    grid.Children.Add(border);
        //    grid.Children.Add(splitter);
        //    grid.Children.Add(separator);
        //}

        grid.RowDefinitions.Add(new RowDefinition(5, GridUnitType.Pixel));
        return grid;
    }





    













    //#region Получить разделитель
    ///// <summary>
    ///// Получить разделитель
    ///// </summary>
    ///// <returns></returns>
    //private static GridSplitter GetGridSplitter()
    //{
    //    return new GridSplitter
    //    {
    //        Background = Brushes.Transparent,
    //        BorderThickness = new(0),
    //        ResizeDirection = GridResizeDirection.Rows,
    //        VerticalAlignment = VerticalAlignment.Bottom,
    //        MinWidth = 0,
    //        Height = 5,
    //        Margin = new(0, 0, 0, 0)
    //    };
    //}
    //#endregion

    //#region Получить визуальный разделитель
    ///// <summary>
    ///// Получить визуальный разделитель
    ///// </summary>
    ///// <returns></returns>
    //private static Rectangle GetViewwSplitter()
    //{
    //    return new Rectangle
    //    {
    //        Fill = SeparatorBrush,
    //        Height = WidthSeparator,
    //        Margin = new(0, 0, 0, 0),
    //        IsHitTestVisible = false,
    //        VerticalAlignment = VerticalAlignment.Bottom
    //    };
    //}
    //#endregion

    //#region Поулчить элемент управления
    ///// <summary>
    ///// Поулчить элемент управления
    ///// </summary>
    ///// <param name="Item"></param>
    ///// <returns></returns>
    //private static TextBlock GetItemControl(ModelRow Item)
    //{
    //    return new TextBlock()
    //    {
    //        Text = Item.Header,
    //        HorizontalAlignment = HorizontalAlignment.Center,
    //        VerticalAlignment = VerticalAlignment.Center,
    //        FontFamily = Item.CellStyle.FontFamily,
    //        FontSize = Item.CellStyle.FontSize,
    //        FontWeight = Item.CellStyle.IsBold ? FontWeight.Bold : FontWeight.Normal,
    //        Foreground = Helper.GetColor(Item.CellStyle.Foreground)
    //    };
    //}
    //#endregion

    private void RebuildContent()
    {
        Content = InitializeContent();
    }

    private void UpdateTransform()
    {
        transform.Y = -PositionY;
        transform.X = -PositionX;
    }

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

    private sealed class RowDefinitionWidthConverter : IValueConverter
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
}