using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;

public class ModelPresentationCell : BaseTemplatedControl
{
    #region Поля класса
    private bool isDoubleClick;
    #endregion

    #region Статический конструктор класса
    /// <summary>
    /// Статический конструктор класса
    /// </summary>
    static ModelPresentationCell()
        => AddClassHandlers();
    #endregion

    #region Источник данных
    public static readonly StyledProperty<ModelCell> ItemSourceProperty =
        AvaloniaProperty.Register<ModelPresentationCell, ModelCell>(nameof(ItemSource));

    /// <summary>
    /// Источник данных
    /// </summary>
    public ModelCell ItemSource
    {
        get => GetValue(ItemSourceProperty);
        set => SetValue(ItemSourceProperty, value);
    }
    #endregion

    #region Модель данных
    public static readonly StyledProperty<ModelTable> ModelProperty =
        AvaloniaProperty.Register<ModelPresentationCell, ModelTable>(nameof(Model));

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
        => InitializeBorder(FindPartById<Border>(CurrTemplateAppliedEventArgs, "PART_Border"));
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

        var editPanel = FindPartById<TextBox>(CurrTemplateAppliedEventArgs, "PART_EditPanel");
        editPanel.Tapped += (_, _) => OnEditPanelTapped();
        editPanel.GetObservable(IsVisibleProperty).Subscribe(new Helper.Observer<bool>(isVisible => OnEditPanelVisible(editPanel)));

        border.PointerPressed += (_, e) => OnBorderPointerPressed(e);
        border.DoubleTapped += (_, _) => OnBorderDoubleTapped(editPanel);
    }
    #endregion

    #region Метод создания обработчиков событий
    /// <summary>
    /// Метод создания обработчиков событий
    /// </summary>
    private static void AddClassHandlers()
    {
        ItemSourceProperty.Changed.AddClassHandler<ModelPresentationCell>((x, _) => x.DataContext = x.ItemSource);
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

    #region Обработка события нажатия ЛКМ на ячейку
    /// <summary>
    /// Обработка события нажатия ЛКМ на ячейку
    /// </summary>
    /// <param name="e"></param>
    private void OnBorderPointerPressed(PointerPressedEventArgs e)
        => Model?.SetSelected(e, ItemSource);
    #endregion

    #region Обработка события двойного клика на ячейку
    /// <summary>
    /// Обработка события двойного клика на ячейку
    /// </summary>
    /// <param name="editPanel"></param>
    private void OnBorderDoubleTapped(TextBox editPanel)
    {
        if (ItemSource.IsEdit)
            return;

        isDoubleClick = true;
        ItemSource.IsEdit = true;
        Model?.EditChangeEvent.Invoke(AppActiveModeType.IsInput);
    }
    #endregion

    #region Обработка события клика ЛКМ на редактируемую ячейку
    /// <summary>
    /// Обработка события клика ЛКМ на редактируемую ячейку
    /// </summary>
    private void OnEditPanelTapped()
    {
        Model?.EditChangeEvent.Invoke(AppActiveModeType.IsEditCell);
    }
    #endregion

    #region Обработка события изменения видимости панели редактирвоания
    /// <summary>
    /// Обработка события изменения видимости панели редактирвоания
    /// </summary>
    private void OnEditPanelVisible(TextBox editPanel)
    {
        if (!editPanel.IsVisible)
        {
            isDoubleClick = false;
            return;
        }

        if (!isDoubleClick)
        {
            editPanel.SelectionStart = editPanel.Text == null ? 0 : editPanel.Text.Length;
            editPanel.SelectionEnd = editPanel.Text == null ? 0 : editPanel.Text.Length;
        }
        else
            editPanel.SelectAll();

        editPanel.Focus();
        Model?.EditChangeEvent.Invoke(AppActiveModeType.IsInput);
    }
    #endregion
}