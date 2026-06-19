using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base.Interfaces;
using AvaloniaTemplate.Infrastructures.Helpers;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ComboBoxWithFontsSize : TemplatedControl
{
    #region Выбранный элемент из спсика
    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, int>(nameof(SelectedIndex), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Выбранный элемент из спсика
    /// </summary>
    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }
    #endregion

    #region Индекс выбранного элемента из спсика
    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Индекс выбранного элемента из спсика
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
        var array = new List<Border>();
        var fontFamily = Helper.GetResource<FontFamily>("FontFamilyDefault");
        List<double> list = [8, 9, 10, 11, 12, 14, 16, 18, 20, 22, 24, 28, 32, 36, 48, 72];

        list.ToList()?
            .ForEach(x =>
            {
                var border = new Border
                {
                    Background = Brushes.Transparent,
                    Child = new TextBlock
                    {
                        Text = x.ToString(),
                        FontFamily = fontFamily,
                        FontSize = 10
                    }
                };
                border.PointerMoved += (s, e)
                                => App.GetService<ICommandProvider>()?
                                .GetCommand("Command_PreViewSelectedFontSize")?
                                .Execute(x);

                array.Add(border);
            });

        SelectedItem = array[3];
        ItemsSource = array;



        var ButtonFontSizeUp = e.NameScope.Find<Button>("PART_ButtonFontSizeUp");
        var ButtonFontSizeDown = e.NameScope.Find<Button>("PART_ButtonFontSizeDown");
        ButtonFontSizeUp.Click += (s, e) =>
        {
            if (SelectedIndex < ItemsSource.Count - 1)
                SelectedIndex++;
        };

        ButtonFontSizeDown.Click += (s, e) =>
        {
            if (SelectedIndex > 0)
                SelectedIndex--;
        };
    }
}