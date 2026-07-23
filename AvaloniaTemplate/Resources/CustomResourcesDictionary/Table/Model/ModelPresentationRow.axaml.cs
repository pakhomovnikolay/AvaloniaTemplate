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

public class ModelPresentationRow : BaseTemplatedControl
{
    #region Статический конструктор класса
    /// <summary>
    /// Статический конструктор класса
    /// </summary>
    static ModelPresentationRow()
        => AddClassHandlers();
    #endregion

    #region Источник данных
    public static readonly StyledProperty<ModelRow> ItemSourceProperty =
        AvaloniaProperty.Register<ModelPresentationRow, ModelRow>(nameof(ItemSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public ModelRow ItemSource
    {
        get => GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }
    #endregion

    #region Модель данных
    public static readonly StyledProperty<ModelTable> ModelProperty =
        AvaloniaProperty.Register<ModelPresentationRow, ModelTable>(nameof(Model));

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

        splitter.DragStarted += (_, _) => Model?.DragStartedChange?.Invoke(Orientation.Horizontal, ItemSource.Geometry.PositionY, ItemSource.Geometry.Bottom);
        splitter.DragCompleted += (_, e) => Model?.RowDragCompletedChange?.Invoke(Orientation.Vertical, ItemSource);
        splitter.DoubleTapped += (_, e) => Model?.RowSplitterDoubleTappedChange?.Invoke(ItemSource);
        splitter.DragDelta += (_, e) => OnDragDelta(e.Vector.Y, splitter.Height, ItemSource);
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

    #region Изменение размера строки
    /// <summary>
    /// Изменение размера строки
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="item"></param>
    private void OnDragDelta(double delta, double minHeight, ModelRow item)
    {
        var minDelta = 0.5;
        if (Math.Abs(delta) < minDelta || item.Geometry.Height <= minHeight && delta < 0)
            return;

        Model.Resize(item, delta);
        Model?.DragStartedChange?.Invoke(Orientation.Horizontal, item.Geometry.PositionY, item.Geometry.Bottom);
    }
    #endregion

    #region Метод создания обработчиков событий
    /// <summary>
    /// Метод создания обработчиков событий
    /// </summary>
    private static void AddClassHandlers()
    {
        ItemSourceProperty.Changed.AddClassHandler<ModelPresentationRow>((x, _) => x.DataContext = x.ItemSource);
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