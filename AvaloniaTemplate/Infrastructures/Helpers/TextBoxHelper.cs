using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using System.Windows.Input;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class TextBoxHelper
    {
        public static readonly AttachedProperty<ICommand> EnterKeyCommandProperty =
            AvaloniaProperty.RegisterAttached<TextBoxHelper, TextBox, ICommand>("EnterKeyCommand");

        public static readonly AttachedProperty<object> EnterKeyCommandParameterProperty =
            AvaloniaProperty.RegisterAttached<TextBoxHelper, TextBox, object>("EnterKeyCommandParameter");

        static TextBoxHelper()
        {
            EnterKeyCommandProperty.Changed.AddClassHandler<TextBox>(OnEnterKeyCommandChanged);
        }

        public static void SetEnterKeyCommand(AvaloniaObject element, ICommand value)
            => element.SetValue(EnterKeyCommandProperty, value);

        public static ICommand GetEnterKeyCommand(AvaloniaObject element)
            => element.GetValue(EnterKeyCommandProperty);


        public static void SetEnterKeyCommandParameter(AvaloniaObject element, object value)
            => element.SetValue(EnterKeyCommandParameterProperty, value);
        public static object GetEnterKeyCommandParameter(AvaloniaObject element)
            => element.GetValue(EnterKeyCommandParameterProperty);

        private static void OnEnterKeyCommandChanged(TextBox textBox, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.OldValue is not ICommand oldCommand || e.NewValue is not ICommand newCommand)
                return;

            if (oldCommand is { })
                textBox.KeyDown -= OnTextBoxKeyDown;

            if (newCommand is { })
                textBox.KeyDown += OnTextBoxKeyDown;
        }

        private static void OnTextBoxKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && sender is TextBox textBox)
            {
                var Command = GetEnterKeyCommand(textBox);
                var p = GetEnterKeyCommandParameter(textBox);
                if (Command?.CanExecute(p) == true)
                    Command?.Execute(p);
            }
        }
    }
}
