using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;
using System.Collections.Generic;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterColumns : BaseTemplatedControl
{
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
    private List<ModelPresentationColumn> Presenters { get; } = [];
    #endregion

    #region Статический констуктор класса
    /// <summary>
    /// Статический констуктор класса
    /// </summary>
    static PresenterColumns()
        => AddClassHandlers();
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
        Transform.Y = PositionY;
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
        Model.UpdateGeometryColumnsFinished += OnUpdateGeometryCellsFinished;
    }
    #endregion

    #region Инициализация контента
    /// <summary>
    /// Инициализация контента
    /// </summary>
    /// <returns></returns>
    private void InitializeContent()
    {
        Presenters.Clear();
        Presenter?.Children.Clear();
        foreach (var column in Model?.ColumnsVisible)
        {
            var presenter = CreateModelPresentationColumn(column);
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
        for (int i = 0; i < Model?.ColumnsVisible.Count; i++)
        {
            if (i >= Presenters.Count)
            {
                var col = CreateModelPresentationColumn(Model?.ColumnsVisible[i]);
                Presenters.Add(col);
                Presenter.Children.Add(col);
            }
            else
            {
                Presenters[i].ItemSource = Model?.ColumnsVisible[i];
            }
        }
        Presenter.InvalidateArrange();
    }
    #endregion

    #region Создать модель представления колонки
    /// <summary>
    /// Создать модель представления колонки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private ModelPresentationColumn CreateModelPresentationColumn(ModelColumn item)
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
        ModelProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.InitializeContent());
        PositionXProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.UpdateTransform());
        ScaleProperty.Changed.AddClassHandler<PresenterColumns>((x, _) => x.InitializeContent());
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
        if (Model is not { } || Model.Columns.Count <= 0)
            return;

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

        Model?.ColumnsPointerMovedEvent(Presenter, e);
    }
    #endregion
}