using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class ToggleGroupHelper
    {
        static ToggleGroupHelper()
        {
            GroupControlIsCheckedChangeProperty.Changed.AddClassHandler<ToggleButton>(RegisterIsChecked);

            //GroupNameProperty.Changed.AddClassHandler<ToggleButton>(OnGroupChanged);
        }

        public static readonly AttachedProperty<string?> GroupControlClickChangeProperty
            = AvaloniaProperty.RegisterAttached<ToggleButton, Control, string?>("Group");

        public static void SetGroupClick(Control control, string? value)
            => control.SetValue(GroupControlClickChangeProperty, value);

        public static string? GetGroupClick(Control control)
            => control.GetValue(GroupControlClickChangeProperty);


        public static readonly AttachedProperty<string?> GroupControlIsCheckedChangeProperty
            = AvaloniaProperty.RegisterAttached<ToggleButton, Control, string?>("Group");

        public static void SetGroupIsChecked(Control control, string? value)
            => control.SetValue(GroupControlIsCheckedChangeProperty, value);

        public static string? GetGroupIsChecked(Control control)
            => control.GetValue(GroupControlIsCheckedChangeProperty);


        private static void RegisterControlClick(ToggleButton tb, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is string)
                tb.Click += OnClickChanged;
            else
                tb.Click -= OnClickChanged;
        }

        private static void OnClickChanged(object? sender, RoutedEventArgs e)
        {
            if (sender is null || sender is not ToggleButton source)
                return;

            if (source.IsChecked != true)
                source.IsChecked = true;
            else
                UncheckOthers(source);
        }

        private static void UncheckOthers(ToggleButton source)
        {
            var sourceIsChecked = source.IsChecked == true;
            var group = GetGroupClick(source);
            if (group is null)
                return;

            // Ищем ближайшего общего контейнера
            var root = source.GetVisualParent();
            while (root is not null)
            {
                var visuals = root.GetVisualDescendants()?.Where(v => v is ToggleButton tb && GetGroupClick(tb) == group)?.ToList();
                foreach (var v in visuals)
                    if (v is ToggleButton tb && !tb.Equals(source))
                        tb.IsChecked = false;

                root = root.GetVisualParent();
            }
        }

        private static void RegisterIsChecked(ToggleButton tb, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is string)
                tb.IsCheckedChanged += OnIsCheckedChanged;
            else
                tb.IsCheckedChanged -= OnIsCheckedChanged;
        }

        private static void OnIsCheckedChanged(object? sender, RoutedEventArgs e)
        {
            if (sender is null || sender is not ToggleButton source || source.IsChecked != true)
                return;

            var group = GetGroupIsChecked(source);
            if (group is null)
                return;

            var root = source.GetVisualParent();
            while (root is not null)
            {
                foreach (var tb in root.GetVisualDescendants()
                                       .OfType<ToggleButton>()
                                       .Where(x => x != source && GetGroupIsChecked(x) == group))
                {
                    tb.IsChecked = false;
                }

                root = root.GetVisualParent();
            }
        }


        //public static readonly AttachedProperty<string?> GroupNameProperty
        //    = AvaloniaProperty.RegisterAttached<ToggleButton, Control, string?>("GroupName");

        //static ToggleGroupControl()
        //    => GroupNameProperty.Changed.AddClassHandler<ToggleButton>(OnGroupChanged);

        //public static void SetGroupName(Control control, string? value)
        //    => control.SetValue(GroupNameProperty, value);

        //public static string? GetGroupName(Control control)
        //    => control.GetValue(GroupNameProperty);

        //private static void OnGroupChanged(ToggleButton tb, AvaloniaPropertyChangedEventArgs e)
        //{
        //    tb.Click -= OnChecked;
        //    tb.Click += OnChecked;
        //}

        //private static void OnChecked(object? sender, RoutedEventArgs e)
        //{
        //    if (sender is ToggleButton tb)
        //    {
        //        if (tb.IsChecked != true)
        //            tb.IsChecked = true;
        //        else
        //            UncheckOthers(tb);
        //    }
        //}

        //private static void UncheckOthers(ToggleButton source)
        //{
        //    var sourceIsChecked = source.IsChecked == true;
        //    var group = GetGroupName(source);
        //    if (group is null)
        //        return;

        //    // Ищем ближайшего общего контейнера
        //    var root = source.GetVisualParent();
        //    while (root is not null)
        //    {
        //        var visuals = root.GetVisualDescendants()?.Where(v => v is ToggleButton tb && GetGroupName(tb) == group)?.ToList();
        //        foreach (var v in visuals)
        //            if (v is ToggleButton tb && !tb.Equals(source))
        //                tb.IsChecked = false;

        //        root = root.GetVisualParent();
        //    }
        //}
    }
}