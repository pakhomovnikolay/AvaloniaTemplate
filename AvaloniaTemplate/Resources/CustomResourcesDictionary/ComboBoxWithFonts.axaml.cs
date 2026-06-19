using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Commands.Base.Interfaces;
using AvaloniaTemplate.Infrastructures.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ComboBoxWithFonts : TemplatedControl
{
    #region Параметр для команды
    public static readonly StyledProperty<object?> SelectedItemProperty =
        AvaloniaProperty.Register<ComboBoxWithFonts, object?>(nameof(SelectedItem), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Параметр для команды
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
        FontManager.Current.SystemFonts?
            .ToList()?
            .ForEach(x =>
            {
                var border = new Border { Background = Brushes.Transparent, Child = new TextBlock { Text = x.Name, FontFamily = x, FontSize = 10 } };
                border.PointerMoved += (s, e)
                                => App.GetService<ICommandProvider>()?
                                .GetCommand("Command_PreViewSelectedFont")?
                                .Execute(x.Name);

                array.Add(border);

                if (x.Name.Contains(Helper.GetResource<FontFamily>("FontFamilyDefault").Name, StringComparison.CurrentCultureIgnoreCase))
                    SelectedItem = border;
            });

        ItemsSource = array;
    }
}