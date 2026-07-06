using Avalonia;
using Avalonia.Controls;
using AvaloniaTemplate.Resources.CustomResourcesDictionary;
using AvaloniaTemplate.Services;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class ScrollBarHelper
    {
        static ScrollBarHelper()
        {
            HorizontalScrollBarValueChangeControlProperty.Changed.AddClassHandler<SelectorScrollBar>(RegisterHorizontalScrollBarValueChangeControl);
            VerticalScrollBarValueChangeControlProperty.Changed.AddClassHandler<SelectorScrollBar>(RegisterVerticalScrollBarValueChangeControl);
        }

        public static readonly AttachedProperty<bool> HorizontalScrollBarValueChangeControlProperty
            = AvaloniaProperty.RegisterAttached<ScrollBarHelper, Control, bool>("HorizontalScrollBarValueChangeControl");

        public static void SetHorizontalScrollBarValueChangeControl(Control control, bool value)
            => control.SetValue(HorizontalScrollBarValueChangeControlProperty, value);

        public static bool GetHorizontalScrollBarValueChangeControl(Control control)
            => control.GetValue(HorizontalScrollBarValueChangeControlProperty);

        public static readonly AttachedProperty<bool> VerticalScrollBarValueChangeControlProperty
            = AvaloniaProperty.RegisterAttached<ScrollBarHelper, Control, bool>("VerticalScrollBarValueChangeControl");

        public static void SetVerticalScrollBarValueChangeControl(Control control, bool value)
            => control.SetValue(VerticalScrollBarValueChangeControlProperty, value);

        public static bool GetVerticalScrollBarValueChangeControl(Control control)
            => control.GetValue(VerticalScrollBarValueChangeControlProperty);

        private static void RegisterHorizontalScrollBarValueChangeControl(SelectorScrollBar scroll, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool)
                scroll.ValueChanged += OnHorizontalScrollBarValueChange;
            else
                scroll.ValueChanged -= OnHorizontalScrollBarValueChange;
        }

        private static void RegisterVerticalScrollBarValueChangeControl(SelectorScrollBar scroll, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool)
                scroll.ValueChanged += OnVerticalScrollBarValueChange;
            else
                scroll.ValueChanged -= OnVerticalScrollBarValueChange;
        }

        private static void OnHorizontalScrollBarValueChange(double value)
        {
            App.GetService<IScrollBarService>()?
                .UpdateHorizontalScrollBarOffset(value);
        }

        private static void OnVerticalScrollBarValueChange(double value)
        {
            App.GetService<IScrollBarService>()?
                .UpdateVerticalScrollBarOffset(value);
        }
    }
}
