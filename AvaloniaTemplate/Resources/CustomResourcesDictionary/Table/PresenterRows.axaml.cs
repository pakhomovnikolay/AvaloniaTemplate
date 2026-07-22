using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;
using System;
using System.Collections.Generic;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterRows : BaseTemplatedControl
{
    private static bool IsMousePressed;
    private readonly TranslateTransform transform = new();
    private SpreadsheetPanel presenter;
    private readonly List<ModelPresentationRow> presenters = [];

    static PresenterRows()
    {
        ModelProperty.Changed.AddClassHandler<PresenterRows>((x, _) => x.RebuildContent());
        PositionXProperty.Changed.AddClassHandler<PresenterRows>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterRows>((x, _) => x.UpdateTransform());
        ScaleProperty.Changed.AddClassHandler<PresenterRows>((x, _) => x.RebuildContent());
    }

    #region Событие изменения текущего элемента
    /// <summary>
    /// Событие изменения текущего элемента
    /// </summary>
    public event Action<PointerPressedEventArgs, ModelRow> SelectedItemChanged;
    #endregion

    #region Событие устанвоки фокуса элемента
    /// <summary>
    /// Событие устанвоки фокуса элемента
    /// </summary>
    public event Action<ModelRow> SetFocusItem;
    #endregion

    #region Событие снятия фокуса элемента
    /// <summary>
    /// Событие снятия фокуса элемента
    /// </summary>
    public event Action<ModelRow> ResetFocusItem;
    #endregion

    #region Событие перемещения мыши по панели
    /// <summary>
    /// Событие перемещения мыши по панели
    /// </summary>
    public event Action<Control?, PointerEventArgs> PointerMovedEventChange;
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

    #region Модель данных
    public static readonly StyledProperty<ModelTable> ModelProperty =
        AvaloniaProperty.Register<PresenterRows, ModelTable>(nameof(Model));

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
        presenter = InitializeContent();
        presenter.RenderTransform = transform;
        Content = presenter;
        Model.UpdateGeometryCellsFinished += OnUpdateGeometryCellsFinished;
    }
    #endregion

    #region Масштаб
    public static readonly StyledProperty<double> ScaleProperty =
        AvaloniaProperty.Register<PresenterRows, double>(nameof(Scale), defaultValue: 1);

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
        transform.Y = -PositionY;
        transform.X = PositionX;
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

    #region Инициализация контента
    /// <summary>
    /// Инициализация контента
    /// </summary>
    /// <returns></returns>
    private SpreadsheetPanel InitializeContent()
    {
        presenters.Clear();
        presenter?.Children.Clear();
        var panel = new SpreadsheetPanel();
        panel.Bind(SpreadsheetPanel.ZoomProperty, new Binding(nameof(Scale)) { Source = this });
        foreach (var row in Model?.RowsVisible)
        {
            var presenter = new ModelPresentationRow() { Model = Model, ItemSource = row };
            presenters.Add(presenter);
            panel.Children.Add(presenter);
        }
        return panel;
    }
    #endregion

    #region Обновить визуальное представление данных
    /// <summary>
    /// Обновить визуальное представление данных
    /// </summary>
    private void UpdateVisibleContent()
    {
        for (int i = 0; i < Model?.RowsVisible.Count; i++)
        {
            if (i >= presenters.Count)
            {
                var row = new ModelPresentationRow() { Model = Model, ItemSource = Model?.RowsVisible[i] };
                presenters.Add(row);
                presenter.Children.Add(row);
            }
            else
            {
                presenters[i].ItemSource = Model?.RowsVisible[i];
            }
        }
        presenter.InvalidateArrange();
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

    #region Изменение размера колонки
    /// <summary>
    /// Изменение размера колонки
    /// </summary>
    /// <param name="delta"></param>
    /// <param name="item"></param>
    private void OnDragDelta(double delta, double minHeight, ModelRow item)
    {
        var minDelta = 0.5;
        if (Math.Abs(delta) < minDelta || item.Geometry.Height <= minHeight)
            return;

        Model.Resize(item, delta);
        Model?.DragStartedChange?.Invoke(Orientation.Horizontal, item.Geometry.PositionY, item.Geometry.Bottom);
        presenter.InvalidateArrange();
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (Model is not { } || Model.Rows.Count <= 0)
            return;

        RebuildContent();
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