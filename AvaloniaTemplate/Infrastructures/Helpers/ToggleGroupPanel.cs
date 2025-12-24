using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class ToggleGroupPanel : StackPanel
    {
        protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
        {
            base.OnAttachedToVisualTree(e);
            foreach (var child in Children.OfType<ToggleButton>())
                child.Click += OnChecked;
        }

        private void OnChecked(object? sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton source)
                return;

            if (source.IsChecked != true)
                source.IsChecked = true;
            else
                foreach (var child in Children.OfType<ToggleButton>())
                    if (child != source)
                        child.IsChecked = false;
        }
    }
}
