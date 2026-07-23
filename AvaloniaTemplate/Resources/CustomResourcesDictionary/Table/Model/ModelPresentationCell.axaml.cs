using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using AvaloniaTemplate.Infrastructures.Converters;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;

public class ModelPresentationCell : BaseTemplatedControl
{
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
    {
        InitializeBorder(FindPartById<Border>(CurrTemplateAppliedEventArgs, "PART_Border"));
        InitializeViewPanel(FindPartById<TextBlock>(CurrTemplateAppliedEventArgs, "PART_ViewPanel"));
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

        viewPanel.Bind(TextBlock.TextProperty, new Binding("VisualValue"));
        viewPanel.Bind(TextBlock.FontFamilyProperty, new Binding("CellStyle.FontFamily") { Converter = new FontFamilyNameConverter() });
        viewPanel.Bind(TextBlock.FontSizeProperty, new Binding("CellStyle.FontSize"));
        viewPanel.Bind(TextBlock.FontWeightProperty, new Binding("CellStyle.IsBold") { Converter = new FontWeightBoldConverter() });
        viewPanel.Bind(TextBlock.ForegroundProperty, new Binding("CellStyle.Foreground") { Converter = new ColorStringToSolidColorBrushConverter() });
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
}