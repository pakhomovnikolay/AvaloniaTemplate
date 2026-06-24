using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Layout;
using AvaloniaTemplate.Models.LayoutControls;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ButtonSetTextPosition : TemplatedControl
{
    #region Позиция по вертикали
    public static readonly StyledProperty<VerticalAlignment> VerticalContentAlignmentProperty =
        AvaloniaProperty.Register<ButtonSetTextPosition, VerticalAlignment>(nameof(VerticalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Позиция по вертикали
    /// </summary>
    public VerticalAlignment VerticalContentAlignment
    {
        get => GetValue(VerticalContentAlignmentProperty);
        set => SetValue(VerticalContentAlignmentProperty, value);
    }
    #endregion

    #region Позиция по горизинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalContentAlignmentProperty =
        AvaloniaProperty.Register<ButtonSetTextPosition, HorizontalAlignment>(nameof(HorizontalContentAlignment), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Позиция по горизинтали
    /// </summary>
    public HorizontalAlignment HorizontalContentAlignment
    {
        get => GetValue(HorizontalContentAlignmentProperty);
        set => SetValue(HorizontalContentAlignmentProperty, value);
    }
    #endregion

    #region Позиция по вертикали
    public static readonly StyledProperty<VerticalAlignment> VerticalPositionProperty =
        AvaloniaProperty.Register<ButtonSetTextPosition, VerticalAlignment>(nameof(VerticalPosition), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Позиция по вертикали
    /// </summary>
    public VerticalAlignment VerticalPosition
    {
        get => GetValue(VerticalPositionProperty);
        set => SetValue(VerticalPositionProperty, value);
    }
    #endregion

    #region Позиция по горизинтали
    public static readonly StyledProperty<HorizontalAlignment> HorizontalPositionProperty =
        AvaloniaProperty.Register<ButtonSetTextPosition, HorizontalAlignment>(nameof(HorizontalPosition), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Позиция по горизинтали
    /// </summary>
    public HorizontalAlignment HorizontalPosition
    {
        get => GetValue(HorizontalPositionProperty);
        set => SetValue(HorizontalPositionProperty, value);
    }
    #endregion

    #region Ориентация
    public static readonly StyledProperty<Orientation> PositionProperty =
        AvaloniaProperty.Register<ButtonSetTextPosition, Orientation>(nameof(Position), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Ориентация
    /// </summary>
    public Orientation Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }
    #endregion

    #region Установлена
    public static readonly StyledProperty<bool> IsCheckedProperty =
        AvaloniaProperty.Register<ButtonSetTextPosition, bool>(nameof(IsChecked), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Установлена
    /// </summary>
    public bool IsChecked
    {
        get => GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }
    #endregion

    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ButtonSetTextPosition, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<ButtonSetTextPosition, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

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
        var button = e.NameScope.Find<Button>("PART_Button");
        var control = new LayoutTextPositionType()
        {
            VerticalPosition = VerticalPosition,
            HorizontalPosition = HorizontalPosition,
            Position = Position,
            //Width = Width,
            //Height = Height,
            //Margin = new(0, 0, 0, 0)
        };
        control.InvalidateVisual();
        button.Content = control;

        if (Position == Orientation.Vertical)
            CommandParameter = VerticalPosition;
        else
            CommandParameter = HorizontalPosition;
    }
}