using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class BorderHelper
    {
        public static readonly AttachedProperty<ICommand> PointerEnteredCommandProperty =
            AvaloniaProperty.RegisterAttached<BorderHelper, Border, ICommand>("PointerEnteredCommand");

        public static readonly AttachedProperty<object?> PointerEnteredCommandParameterProperty =
            AvaloniaProperty.RegisterAttached<BorderHelper, Border, object?>("PointerEnteredCommandParameter");

        static BorderHelper()
        {
            PointerEnteredCommandProperty.Changed.AddClassHandler<Border>(RegistryPointerEnteredCommand);
        }

        public static void SetPointerEnteredCommand(AvaloniaObject element, ICommand value)
            => element.SetValue(PointerEnteredCommandProperty, value);

        public static ICommand GetPointerEnteredCommand(AvaloniaObject element)
            => element.GetValue(PointerEnteredCommandProperty);

        public static void SetPointerEnteredCommandParameter(AvaloniaObject element, object value)
            => element.SetValue(PointerEnteredCommandParameterProperty, value);

        public static object? GetPointerEnteredCommandParameter(AvaloniaObject element)
            => element.GetValue(PointerEnteredCommandParameterProperty);



        private static void RegistryPointerEnteredCommand(Border border, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is ICommand)
                border.PointerEntered += OnPointerEntered;
            else
                border.PointerEntered -= OnPointerEntered;
        }

        private static void OnPointerEntered(object? sender, PointerEventArgs e)
        {
            if (sender is not Border border)
                return;

            var command = GetPointerEnteredCommand(border);
            var commandParameter = GetPointerEnteredCommandParameter(border);
            command?.Execute(commandParameter);

        }
    }
}
