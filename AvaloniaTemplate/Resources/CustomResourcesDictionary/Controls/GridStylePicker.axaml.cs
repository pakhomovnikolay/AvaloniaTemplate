using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using AvaloniaTemplate.Infrastructures.Commands.Base;
using AvaloniaTemplate.Infrastructures.Helpers;
using AvaloniaTemplate.Models.Enums;
using AvaloniaTemplate.Models.Enums.TemplatedControlTypes;
using AvaloniaTemplate.Resources.CustomResourcesDictionary.Base;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Controls;

public class GridStylePicker : BaseTemplatedControl
{
    static GridStylePicker()
    {
        BorderStyleTypeProperty.Changed.AddClassHandler<GridStylePicker>((x, _) => x.RenderGridStyle(x.BorderStyleType));
    }

    #region Тип границы
    public static readonly StyledProperty<CurrentBorderStyleType> BorderStyleTypeProperty =
        AvaloniaProperty.Register<GridStylePicker, CurrentBorderStyleType>(nameof(BorderStyleType));

    /// <summary>
    /// Тип границы
    /// </summary>
    public CurrentBorderStyleType BorderStyleType
    {
        get => GetValue(BorderStyleTypeProperty);
        set => SetValue(BorderStyleTypeProperty, value);
    }
    #endregion

    #region Положение контента по горинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<GridStylePicker, HorizontalAlignment>(nameof(HorizontalContentAlignment));

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
        AvaloniaProperty.Register<GridStylePicker, VerticalAlignment>(nameof(VerticalContentAlignment));

    /// <summary>
    /// Положение контента по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Источник данных
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<GridStylePicker, object?>(nameof(Content), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных
    /// </summary>
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }
    #endregion

    #region Окно раскрыто
    public static readonly StyledProperty<bool> IsPopupOpenProperty =
        AvaloniaProperty.Register<GridStylePicker, bool>(nameof(IsPopupOpen), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Окно раскрыто
    /// </summary>
    public bool IsPopupOpen
    {
        get => GetValue(IsPopupOpenProperty);
        set => SetValue(IsPopupOpenProperty, value);
    }
    #endregion

    #region Источник данных раскрывающегося окна
    public static readonly StyledProperty<Panel?> ContentPopupProperty =
        AvaloniaProperty.Register<GridStylePicker, Panel?>(nameof(ContentPopup), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Источник данных раскрывающегося окна
    /// </summary>
    public Panel? ContentPopup
    {
        get => GetValue(ContentPopupProperty);
        set => SetValue(ContentPopupProperty, value);
    }
    #endregion

    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<GridStylePicker, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Команда
    /// </summary>
    public ICommand Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    #endregion

    #region Параметр для команды
    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<GridStylePicker, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var frame = e.NameScope.Find<Popup>("PART_Popup");
        var stackPanel = Helper.CreateStackPanel(Orientation.Vertical, 5);
        stackPanel.Children.Add(Helper.CreateLabel("Границы"));
        GridStyleHelper.CreateGridStyle(Command, stackPanel, frame);

        ContentPopup ??= new()
        {
            Width = 230,
            Children = { stackPanel }
        };

        //RenderGridStyle(CurrentBorderStyleType.Bottom);
        //Content ??=

        //stackPanel.Children.Add(Helper.CreateLabel("Границы"));
        //GridStyleHelper.CreateGridStyle(Command, stackPanel, frame);

        //PopupContent = new() { Width = 230 };
        //PopupContent.Children.Add(stackPanel);
    }

    private void RenderGridStyle(CurrentBorderStyleType borderStyleType)
    {
        Content = GridStyleHelper.CreateGridStyle(borderStyleType);
    }
}