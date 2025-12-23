using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class ToggleGroupPanel : StackPanel
    {
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);

            foreach (var child in Children.OfType<ToggleButton>())
            {
                child.IsCheckedChanged += OnChecked;
            }
        }

        private void OnChecked(object? sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton source)
                return;

            foreach (var child in Children.OfType<ToggleButton>())
            {
                if (child != source)
                    child.IsChecked = false;
            }

            if (Children.OfType<ToggleButton>()?.FirstOrDefault(t => t.IsChecked == true) is null)
                source.IsChecked = true;
        }
    }





    //public class ToggleGroupControl
    //{
    //    public static readonly AttachedProperty<string?> GroupNameProperty =
    //    AvaloniaProperty.RegisterAttached<ToggleButton, Control, string?>(
    //        "GroupName");

    //    static ToggleGroupControl()
    //    {
    //        GroupNameProperty.Changed.AddClassHandler<ToggleButton>(OnGroupChanged);
    //    }

    //    public static void SetGroupName(Control control, string? value)
    //        => control.SetValue(GroupNameProperty, value);

    //    public static string? GetGroupName(Control control)
    //        => control.GetValue(GroupNameProperty);

    //    private static void OnGroupChanged(ToggleButton tb, AvaloniaPropertyChangedEventArgs e)
    //    {
    //        tb.IsCheckedChanged -= OnChecked;
    //        tb.IsCheckedChanged += OnChecked;
    //    }

    //    private static void OnChecked(object? sender, RoutedEventArgs e)
    //    {
    //        if (sender is ToggleButton tb)
    //            UncheckOthers(tb);
    //    }

    //    private static void UncheckOthers(ToggleButton source)
    //    {
    //        var group = GetGroupName(source);
    //        if (group is null)
    //            return;

    //        var parent = source.Parent;
    //        if (parent is not Panel panel)
    //            return;

    //        foreach (var child in panel.Children)
    //        {
    //            if (child is ToggleButton tb &&
    //                tb != source &&
    //                GetGroupName(tb) == group)
    //            {
    //                tb.IsChecked = false;
    //            }
    //        }
    //    }
    //}
}
