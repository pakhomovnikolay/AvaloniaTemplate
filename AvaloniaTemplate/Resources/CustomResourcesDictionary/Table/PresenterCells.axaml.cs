using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Models.LayoutControls;
using AvaloniaTemplate.Models.LayoutControls.Models;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterCells : BaseTemplatedControl
{
    #region Поля класса
    private Rect? _start;
    private Rect? _end;
    private Rect? _selectedArea;
    private List<Rect?>? _selectedAreas = [];
    #endregion

    #region Свойства класса
    /// <summary>
    /// Состояние нажатой ЛКМ
    /// </summary>
    private bool IsMousePressed { get; set; }

    /// <summary>
    /// Элемент перемещения панели
    /// </summary>
    private TranslateTransform Transform { get; } = new();

    /// <summary>
    /// Панель представления данных
    /// </summary>
    private SpreadsheetPanel Presenter { get; } = new();

    /// <summary>
    /// Коллекция панелей представления данных
    /// </summary>
    private List<ModelPresentationCell> Presenters { get; } = [];

    /// <summary>
    /// Цвет границ веделнной области
    /// </summary>
    private IBrush BorderBrushActiveArea { get; } = new SolidColorBrush(Color.FromRgb(170, 110, 110));

    /// <summary>
    /// Слой отображения области изменения размеров
    /// </summary>
    private LayoutDragArea DragArea { get; } = new() { ZIndex = 9, IsHitTestVisible = false };

    /// <summary>
    /// Слой отображения выделенной области
    /// </summary>
    private LayoutSelectedArea SelectedArea { get; } = new() { ZIndex = 8, IsHitTestVisible = false };

    /// <summary>
    /// Слой отображения выбранных элементов
    /// </summary>
    private LayoutActiveArea ActiveArea { get; } = new() { ZIndex = 7, Opacity = 0.3, IsHitTestVisible = false };
    #endregion

    #region Статический констуктор класса
    /// <summary>
    /// Статический констуктор класса
    /// </summary>
    static PresenterCells()
        => AddClassHandlers();
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

    #region Модель данных
    public static readonly StyledProperty<ModelTable> ModelProperty =
        AvaloniaProperty.Register<PresenterCells, ModelTable>(nameof(Model));

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

    #region Масштаб
    public static readonly StyledProperty<double> ScaleProperty =
        AvaloniaProperty.Register<PresenterCells, double>(nameof(Scale), defaultValue: 1);

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
        Transform.Y = -PositionY;
        Transform.X = -PositionX;
        UpdateVisibleContent();
    }
    #endregion

    #region Обновление положений завершено
    /// <summary>
    /// Обновление положений завершено
    /// </summary>
    private void OnUpdateGeometryCellsFinished()
    {
        UpdateVisibleContent();
    }
    #endregion

    #region Пересобрать контент
    /// <summary>
    /// Пересобрать контент
    /// </summary>
    private void RebuildContent()
    {
        InitializeContent();
        Presenter.Bind(SpreadsheetPanel.ZoomProperty, new Binding(nameof(Scale)) { Source = this });
        Presenter.RenderTransform = Transform;
        Content = Presenter;
        Model.UpdateGeometryCellsFinished += OnUpdateGeometryCellsFinished;
        Model.DragAreaChangeEvent += OnDragAreaChange;
        Model.ActiveAreaChangeEvent += OnActiveAreaChange;
        Model.SelectedAreaChangeEvent += OnSelectedAreaChange;
    }
    #endregion

    #region Инициализация контента
    /// <summary>
    /// Инициализация контента
    /// </summary>
    /// <returns></returns>
    private void InitializeContent()
    {
        Presenters?.Clear();
        Presenter?.Children.Clear();
        foreach (var cell in Model?.CellsVisible)
        {
            var presenter = CreateModelPresentationCell(cell);
            Presenters.Add(presenter);
            Presenter.Children.Add(presenter);
        }
    }
    #endregion

    #region Обновить визуальное представление данных
    /// <summary>
    /// Обновить визуальное представление данных
    /// </summary>
    private void UpdateVisibleContent()
    {
        for (int i = 0; i < Model?.CellsVisible.Count; i++)
        {
            if (i >= Presenters.Count)
            {
                var cell = CreateModelPresentationCell(Model?.CellsVisible[i]);
                Presenters.Add(cell);
                Presenter.Children.Add(cell);
            }
            else
                Presenters[i].ItemSource = Model?.CellsVisible[i];
        }
        Presenter.InvalidateArrange();

        if (DragArea.Area is { })
        {

            DragArea.Area.Start = new(
                _start.Value.X - PositionX,
                _start.Value.Y - PositionY,
                _start.Value.Width,
                _start.Value.Height
                );

            DragArea.Area.End = new(
                _end.Value.X - PositionX,
                _end.Value.Y - PositionY,
                _end.Value.Width,
                _end.Value.Height
                );

            DragArea.InvalidateVisual();
        }
        if (SelectedArea.SelectedArea is { })
        {
            SelectedArea.SelectedArea.Area = new(
                _selectedArea.Value.X - PositionX,
                _selectedArea.Value.Y - PositionY,
                _selectedArea.Value.Width,
                _selectedArea.Value.Height);

            SelectedArea.InvalidateVisual();
        }
        if (ActiveArea.SelectedAreas is { } && ActiveArea.SelectedAreas.Count > 0)
        {
            for (int i = 0; i < ActiveArea.SelectedAreas.Count; i++)
            {
                ActiveArea.SelectedAreas[i].Area = new(
                    _selectedAreas[i].Value.X - PositionX,
                    _selectedAreas[i].Value.Y - PositionY,
                    _selectedAreas[i].Value.Width,
                    _selectedAreas[i].Value.Height);
            }
            ActiveArea.InvalidateVisual();
        }
    }
    #endregion

    #region Создать модель представления строки
    /// <summary>
    /// Создать модель представления строки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private ModelPresentationCell CreateModelPresentationCell(ModelCell item)
        => new()
        {
            Model = Model,
            ItemSource = item
        };
    #endregion

    #region Метод создания обработчиков событий
    /// <summary>
    /// Метод создания обработчиков событий
    /// </summary>
    private static void AddClassHandlers()
    {
        ModelProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.RebuildContent());
        PositionXProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.UpdateTransform());
        ScaleProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.RebuildContent());
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
        if (Model is not { } || Model.Rows.Count <= 0 || Model.Rows.FirstOrDefault(r => r.Cells.Count > 0) is not { })
            return;

        var panel = FindPartById<Panel>(e, "PART_RootPanel");
        panel.Children.Add(DragArea);
        panel.Children.Add(SelectedArea);
        panel.Children.Add(ActiveArea);

        RebuildContent();
    }
    #endregion

    #region Обработка события нажатия КМ
    /// <summary>
    /// Обработка события нажатия КМ
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (!e.Properties.IsLeftButtonPressed)
            return;

        IsMousePressed = true;
    }
    #endregion

    #region Обработка события отпускания КМ
    /// <summary>
    /// Обработка события отпускания КМ
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        if (!IsMousePressed)
            return;

        base.OnPointerReleased(e);
        IsMousePressed = false;
    }
    #endregion

    #region Обработка события перемещения казателя мыши на панеле представления
    /// <summary>
    /// Обработка события перемещения казателя мыши на панеле представления
    /// </summary>
    /// <param name="e"></param>
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (!IsMousePressed)
            return;

        Model?.CellsPointerMovedEvent(Presenter, e);
    }
    #endregion

    #region Обработка изменения области изменения размера
    /// <summary>
    /// Обработка изменения области изменения размера
    /// </summary>
    /// <param name="flow"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    private void OnDragAreaChange(Orientation flow, Rect? start, Rect? end)
    {
        _start = start;
        _end = end;
        DragArea.Area = null;
        if (start is { } && end is { })
        {
            DragArea.Area = new()
            {
                BorderBrush = Brushes.Black,
                Flow = flow,
                Start = new(start.Value.X, start.Value.Y, start.Value.Width, start.Value.Height),
                End = new(end.Value.X, end.Value.Y, end.Value.Width, end.Value.Height)
            };
        }
        DragArea.InvalidateVisual();
    }
    #endregion

    #region Обработка изменения области выбранной ячейки
    /// <summary>
    /// Обработка изменения области выбранной ячейки
    /// </summary>
    /// <param name="rect"></param>
    private void OnSelectedAreaChange(Rect? rect)
    {
        _selectedArea = rect;
        SelectedArea.SelectedArea = null;
        if (rect is { })
        {
            var posX = rect.Value.X - PositionX;
            var posY = rect.Value.Y - PositionY;
            SelectedArea.SelectedArea = new()
            {
                Area = new(posX, posY, rect.Value.Width, rect.Value.Height),
                RectFill = Brushes.Transparent,
                RectPen = new(BorderBrushActiveArea, 2)
            };
        }
        SelectedArea.InvalidateVisual();
    }
    #endregion

    #region Обработка изменения выделенной области
    /// <summary>
    /// Обработка изменения выделенной области
    /// </summary>
    /// <param name="rects"></param>
    private void OnActiveAreaChange(List<Rect?>? rects)
    {
        _selectedAreas = rects;
        ActiveArea?.SelectedAreas.Clear();
        ActiveArea?.SelectedAreas?.AddRange(rects?.Select(x => new LayoutModelActiveArea()
        {
            Area = new Rect(x.Value.X - PositionX, x.Value.Y - PositionY, x.Value.Width, x.Value.Height),
            RectPen = null,
            RectFill = Brushes.LightGray
        }));
        ActiveArea.InvalidateVisual();
    }
    #endregion
}