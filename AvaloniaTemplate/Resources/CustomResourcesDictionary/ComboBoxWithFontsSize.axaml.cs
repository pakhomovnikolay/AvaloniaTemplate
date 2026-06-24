using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using System.Collections;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ComboBoxWithFontsSize : TemplatedControl
{
    static ComboBoxWithFontsSize()
    {
        SelectedItemProperty.Changed.AddClassHandler<ComboBoxWithFontsSize>((x, _) => x.OnSelectedItemChanged());
    }

    #region Индекс выбранного элемента из спсика
    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, int>(nameof(SelectedIndex), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Индекс выбранного элемента из спсика
    /// </summary>
    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }
    #endregion

    #region Выбранный элемент из спсика
    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент из спсика
    /// </summary>
    public object? SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<IList?> ItemsSourceProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, IList?>(nameof(ItemsSource), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных
    /// </summary>
    public IList? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }
    #endregion

    #region Команда смены размера шрифта
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Команда смены размера шрифта
    /// </summary>
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    #endregion

    #region Команда для реализации предварительного просмотра выделенного размера шрифта
    public static readonly StyledProperty<ICommand> CommandPreviewProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, ICommand>(nameof(CommandPreview), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Команда для реализации предварительного просмотра выделенного размера шрифта
    /// </summary>
    public ICommand CommandPreview
    {
        get => GetValue(CommandPreviewProperty);
        set => SetValue(CommandPreviewProperty, value);
    }
    #endregion

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, HorizontalAlignment>(nameof(HorizontalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Положение контента по горинтали
    /// </summary>
    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }
    #endregion

    #region Положение контента по вертикали
    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, VerticalAlignment>(nameof(VerticalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        ItemsSource = FontFamilyHelper.FontSizes;
        SelectedItem = FontFamilyHelper.FontSizeDefault;
        SelectedIndex = FontFamilyHelper.FontSizes.IndexOf(FontFamilyHelper.FontSizeDefault);

        var CBox = e.NameScope.Find<ComboBox>("PART_RootPanel");
        CBox.ItemTemplate = new FuncDataTemplate<double>((size, _) =>
        {
            var border = new Border
            {
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Padding = new Thickness(0),
                Child = new TextBlock
                {
                    Text = $"{size}"
                }
            };
            border.PointerEntered += (_, _) => CommandPreview?.Execute(size);
            border.PointerExited += (_, _) => CommandPreview?.Execute(SelectedItem);
            return border;
        });

        var ButtonFontSizeUp = e.NameScope.Find<Button>("PART_ButtonFontSizeUp");
        ButtonFontSizeUp.Click += (s, e) =>
        {
            if (SelectedIndex < ItemsSource.Count - 1)
                SelectedIndex++;
        };

        var ButtonFontSizeDown = e.NameScope.Find<Button>("PART_ButtonFontSizeDown");
        ButtonFontSizeDown.Click += (s, e) =>
        {
            if (SelectedIndex > 0)
                SelectedIndex--;
        };
    }

    private void OnSelectedItemChanged()
    {
        if (IsLoaded)
            Command?.Execute(SelectedItem);
    }
}