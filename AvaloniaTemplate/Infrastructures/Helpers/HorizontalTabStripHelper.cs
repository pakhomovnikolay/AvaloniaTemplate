using Avalonia;
using Avalonia.Controls;
using AvaloniaTemplate.Resources.CustomResourcesDictionary;
using System.Diagnostics;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class HorizontalTabStripHelper
    {
        static HorizontalTabStripHelper()
        {
            SelectedChangeControlProperty.Changed.AddClassHandler<HorizontalTabStrip>(RegisterSelectedChangeControl);
        }

        public static readonly AttachedProperty<bool> SelectedChangeControlProperty
            = AvaloniaProperty.RegisterAttached<HorizontalTabStrip, Control, bool>("SelectedChangeControl");

        public static void SetSelectedChangeControl(Control control, bool value)
            => control.SetValue(SelectedChangeControlProperty, value);

        public static bool GetSelectedChangeControl(Control control)
            => control.GetValue(SelectedChangeControlProperty);


        private static void RegisterSelectedChangeControl(HorizontalTabStrip tab, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool)
                tab.SelectedItemChange += (x) => OnSelectedItemChange(x, tab);
            else
                tab.SelectedItemChange -= (x) => OnSelectedItemChange(x, tab);
        }

        private static void OnSelectedItemChange(object sender, HorizontalTabStrip source)
        {
            if (sender is null || source is null)
                return;

            Debug.WriteLine($"sender: {sender}");
            Debug.WriteLine($"SelectedItem: {source.SelectedItem}");
            Debug.WriteLine($"SelectedIndex: {source.SelectedIndex}");
        }
    }
}
