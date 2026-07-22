using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Media;
using AvaloniaTemplate.Models.LayoutControls;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table;

public class PresenterCells : BaseTemplatedControl
{
    private static bool IsMousePressed;
    private readonly TranslateTransform transform = new();
    private SpreadsheetPanel presenter;
    private readonly List<ModelPresentationCell> presenters = [];

    static PresenterCells()
    {
        ModelProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.RebuildContent());
        PositionXProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.UpdateTransform());
        PositionYProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.UpdateTransform());
        ScaleProperty.Changed.AddClassHandler<PresenterCells>((x, _) => x.RebuildContent());
    }

    #region Источник данных
    public static readonly StyledProperty<ModelTable> ModelProperty =
        AvaloniaProperty.Register<PresenterCells, ModelTable>(nameof(Model));

    /// <summary>
    /// Источник данных
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
        transform.Y = -PositionY;
        transform.X = -PositionX;
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
        presenter = InitializeContent();
        presenter.RenderTransform = transform;
        Content = presenter;
        Model.UpdateGeometryCellsFinished += OnUpdateGeometryCellsFinished;
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
            foreach (var cell in row.CellsVisible)
            {
                var presenter = CreateModelPresentationColumn(cell);
                presenters.Add(presenter);
                panel.Children.Add(presenter);
            }
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
        var index = 0;
        for (int i = 0; i < Model?.RowsVisible.Count; i++)
        {
            for (int j = 0; j < Model?.ColumnsVisible.Count; j++)
            {
                if (index >= presenters.Count)
                {
                    var cell = CreateModelPresentationColumn(Model?.RowsVisible[i].CellsVisible[j]);
                    presenters.Add(cell);
                    presenter.Children.Add(cell);
                }
                else
                {
                    presenters[index].ItemSource = Model?.RowsVisible[i].CellsVisible[j];
                }
                index++;
            }
        }
        presenter.InvalidateArrange();
    }
    #endregion

    #region Создать модель представления строки
    /// <summary>
    /// Создать модель представления строки
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    private ModelPresentationCell CreateModelPresentationColumn(ModelCell item)
        => new()
        {
            Model = Model,
            ItemSource = item
        };
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

        //SelectedItemChanged?.Invoke(e, item);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        if (Model is not { } || Model.Rows.Count <= 0 || Model.Rows.FirstOrDefault(r => r.Cells.Count > 0) is not { })
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

        //PointerMovedEventChange?.Invoke(presenter, e);
    }
}