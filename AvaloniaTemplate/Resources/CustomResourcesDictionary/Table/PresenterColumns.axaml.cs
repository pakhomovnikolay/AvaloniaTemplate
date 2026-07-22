using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Models.LayoutControls;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;
using System;
using System.Collections.Generic;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterColumns : BaseTemplatedControl
{
    private bool IsMousePressed;
    private readonly TranslateTransform transform = new();
    private SpreadsheetPanel presenter;
    private readonly List<ModelPresentationColumn> presenters = [];

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
        UpdateVisibleContent();
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
        foreach (var column in Model?.ColumnsVisible)
        {
            var presenter = new ModelPresentationColumn() { Model = Model, ItemSource = column };
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
        for (int i = 0; i < Model?.ColumnsVisible.Count; i++)
        {
            if (i >= presenters.Count)
                break;
            presenters[i].ItemSource = Model?.ColumnsVisible[i];
            if (presenter.Children[i] is ModelPresentationColumn column)
                column.ItemSource = Model?.ColumnsVisible[i];
        }
        presenter.InvalidateArrange();
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
        if (Math.Abs(delta) < minDelta || item.Geometry.Width <= minWidth)
            return;

        Model.Resize(item, delta);
        Model?.DragStartedChange?.Invoke(Orientation.Vertical, item.Geometry.PositionX, item.Geometry.Right);
        presenter.InvalidateArrange();
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (Model is not { } || Model.Columns.Count <= 0)
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

        Model?.ColumnsPointerMovedEvent(presenter, e);
    }
}