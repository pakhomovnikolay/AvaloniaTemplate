using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using System.Windows.Input;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary;

public class ButtonCut : TemplatedControl
{
    #region Команда
    public static readonly StyledProperty<ICommand> CommandProperty =
        AvaloniaProperty.Register<ButtonCut, ICommand>(nameof(Command), defaultBindingMode: BindingMode.TwoWay);

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
        AvaloniaProperty.Register<ButtonCut, object?>(nameof(CommandParameter), defaultBindingMode: BindingMode.TwoWay);

    /// <summary>
    /// Параметр для команды
    /// </summary>
    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    #endregion
}