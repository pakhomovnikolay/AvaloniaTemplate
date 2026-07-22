using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using AvaloniaTemplate.Infrastructures.Converters;
using AvaloniaTemplate.Models.SourceTable.Model;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Table.Model;

public class ModelPresentationRow : BaseTemplatedControl
{
    static ModelPresentationRow()
    {
        ItemSourceProperty.Changed.AddClassHandler<ModelPresentationRow>((x, _) => x.DataContext = x.ItemSource);
    }

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

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        InitializeBorder(FindPartById<Border>(e, "PART_Border"));
        InitializeViewPanel(FindPartById<TextBlock>(e, "PART_ViewPanel"));
    }

    private void InitializeBorder(Border border)
    {
        if (border is not { })
            return;

        //border.Bind(DataContextProperty, new Binding(nameof(ItemSource)));
        border.Bind(Border.BackgroundProperty, new Binding("CellStyle.Background") { Converter = new ColorStringToSolidColorBrushConverter() });
        border.Bind(IsVisibleProperty, new Binding("IsVisible"));
        border.PointerEntered += (_, _) => Model?.SetFocus(ItemSource);
        border.PointerExited += (_, _) => Model?.ResetFocus(ItemSource);
        border.PointerPressed += (_, e) => Model?.SetSelected(e, ItemSource);
    }

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
}