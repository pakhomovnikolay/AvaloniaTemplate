using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using AvaloniaTemplate.Infrastructures.Converters;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;

public class ModelPresentationColumn : BaseTemplatedControl
{
    #region Статический конструктор класса
    /// <summary>
    /// Статический конструктор класса
    /// </summary>
    static ModelPresentationColumn()
        => AddClassHandlers();
    #endregion

    #region Источник данных
    public static readonly StyledProperty<ModelColumn> ItemSourceProperty =
        AvaloniaProperty.Register<ModelPresentationColumn, ModelColumn>(nameof(ItemSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public ModelColumn ItemSource
    {
        get => GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }
    #endregion

    #region Модель данных
    public static readonly StyledProperty<ModelTable> ModelProperty =
        AvaloniaProperty.Register<ModelPresentationColumn, ModelTable>(nameof(Model));

    /// <summary>
    /// Модель данных
    /// </summary>
    public ModelTable Model
    {
        get => GetValue(ModelProperty);
        set => SetValue(ModelProperty, value);
    }
    #endregion

    #region Инициализация компонентов
    /// <summary>
    /// Инициализация компонентов
    /// </summary>
    private void InitializeComponents()
    {
        InitializeBorder(FindPartById<Border>(CurrTemplateAppliedEventArgs, "PART_Border"));
        InitializeViewPanel(FindPartById<TextBlock>(CurrTemplateAppliedEventArgs, "PART_ViewPanel"));
        var splitter = FindPartById<GridSplitter>(CurrTemplateAppliedEventArgs, "PART_GridSplitter");

        splitter.DragStarted += (_, _) => Model?.DragStartedChange?.Invoke(Orientation.Vertical, ItemSource.Geometry.PositionX, ItemSource.Geometry.Right);
        splitter.DragCompleted += (_, _) => Model?.ColumnDragCompletedChange?.Invoke(Orientation.Vertical, ItemSource);
        splitter.DoubleTapped += (_, e) => Model?.ColumnSplitterDoubleTappedChange?.Invoke(ItemSource);
        splitter.DragDelta += (_, e) => OnDragDelta(e.Vector.X, splitter.Width, ItemSource);
    }
    #endregion

    #region Инициализация границ элемента
    /// <summary>
    /// Инициализация границ элемента
    /// </summary>
    /// <param name="border"></param>
    private void InitializeBorder(Border border)
    {
        if (border is not { })
            return;

        border.Bind(Border.BackgroundProperty, new Binding("CellStyle.Background") { Converter = new ColorStringToSolidColorBrushConverter() });
        border.Bind(IsVisibleProperty, new Binding("IsVisible"));
        border.PointerEntered += (_, _) => Model?.SetFocus(ItemSource);
        border.PointerExited += (_, _) => Model?.ResetFocus(ItemSource);
        border.PointerPressed += (_, e) => Model?.SetSelected(e, ItemSource);
    }
    #endregion

    #region Инициализация панели отображения данных
    /// <summary>
    /// Инициализация панели отображения данных
    /// </summary>
    /// <param name="viewPanel"></param>
    private static void InitializeViewPanel(TextBlock viewPanel)
    {
        if (viewPanel is not { })
            return;

        viewPanel.Bind(TextBlock.TextProperty, new Binding("Header"));
        viewPanel.Bind(TextBlock.FontFamilyProperty, new Binding("CellStyle.FontFamily") { Converter = new FontFamilyNameConverter() });
        viewPanel.Bind(TextBlock.FontSizeProperty, new Binding("CellStyle.FontSize"));
        viewPanel.Bind(TextBlock.FontWeightProperty, new Binding("CellStyle.IsBold") { Converter = new FontWeightBoldConverter() });
        viewPanel.Bind(TextBlock.ForegroundProperty, new Binding("CellStyle.Foreground") { Converter = new ColorStringToSolidColorBrushConverter() });
    }
    #endregion

    #region Изменение размера колонки
    /// <summary>
    /// Изменение размера колонки
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="item"></param>
    private void OnDragDelta(double delta, double minWidth, ModelColumn item)
    {
        var minDelta = 0.5;
        if (Math.Abs(delta) < minDelta || (item.Geometry.Width <= minWidth && delta < 0))
            return;

        Model.Resize(item, delta);
        Model?.DragStartedChange?.Invoke(Orientation.Vertical, item.Geometry.PositionX, item.Geometry.Right);
    }
    #endregion

    #region Метод создания обработчиков событий
    /// <summary>
    /// Метод создания обработчиков событий
    /// </summary>
    private static void AddClassHandlers()
    {
        ItemSourceProperty.Changed.AddClassHandler<ModelPresentationColumn>((x, _) => x.DataContext = x.ItemSource);
    }
    #endregion

    #region Обработка события применения шаблона
    /// <summary>
    /// Обработка события применения шаблона
    /// </summary>
    /// <param name="e"></param>
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        SetTemplateAppliedEventArgs(e);
        InitializeComponents();
    }
    #endregion
}