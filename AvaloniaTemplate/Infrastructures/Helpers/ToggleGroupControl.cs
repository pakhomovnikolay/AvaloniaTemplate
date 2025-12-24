using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class ToggleGroupControl
    {
        public static readonly AttachedProperty<string?> GroupNameProperty
            = AvaloniaProperty.RegisterAttached<ToggleButton, Control, string?>("GroupName");

        static ToggleGroupControl()
            => GroupNameProperty.Changed.AddClassHandler<ToggleButton>(OnGroupChanged);

        public static void SetGroupName(Control control, string? value)
            => control.SetValue(GroupNameProperty, value);

        public static string? GetGroupName(Control control)
            => control.GetValue(GroupNameProperty);

        private static void OnGroupChanged(ToggleButton tb, AvaloniaPropertyChangedEventArgs e)
        {
            tb.Click -= OnChecked;
            tb.Click += OnChecked;
        }

        private static void OnChecked(object? sender, RoutedEventArgs e)
        {
            if (sender is ToggleButton tb)
            {
                if (tb.IsChecked != true)
                    tb.IsChecked = true;
                else
                    UncheckOthers(tb);
            }
        }

        private static void UncheckOthers(ToggleButton source)
        {
            var sourceIsChecked = source.IsChecked == true;
            var group = GetGroupName(source);
            if (group is null)
                return;

            // Ищем ближайшего общего контейнера
            var root = source.GetVisualParent();
            while (root is not null)
            {
                var visuals = root.GetVisualDescendants()?.Where(v => v is ToggleButton tb && GetGroupName(tb) == group)?.ToList();
                foreach (var v in visuals)
                    if (v is ToggleButton tb && !tb.Equals(source))
                        tb.IsChecked = false;

                root = root.GetVisualParent();
            }
        }
    }
}
