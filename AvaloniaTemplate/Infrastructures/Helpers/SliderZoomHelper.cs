using Avalonia;
using Avalonia.Controls;
using AvaloniaTemplate.Resources.CustomResourcesDictionary;
using AvaloniaTemplate.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Infrastructures.Helpers
{
    public class SliderZoomHelper
    {
        static SliderZoomHelper()
        {
            ValueChangeControlProperty.Changed.AddClassHandler<SliderZoomControl>(RegisterValueChangeControl);
        }

        public static readonly AttachedProperty<bool> ValueChangeControlProperty
            = AvaloniaProperty.RegisterAttached<SliderZoomHelper, Control, bool>("ValueChangeControl");

        public static void SetValueChangeControl(Control control, bool value)
            => control.SetValue(ValueChangeControlProperty, value);

        public static bool GetValueChangeControl(Control control)
            => control.GetValue(ValueChangeControlProperty);

        private static void RegisterValueChangeControl(SliderZoomControl scroll, AvaloniaPropertyChangedEventArgs e)
        {
            if (e.NewValue is bool)
                scroll.ValueChanged += OnValueChange;
            else
                scroll.ValueChanged -= OnValueChange;
        }

        private static void OnValueChange(double value)
        {
            App.GetService<IZoomService>()?
                .SetScale(value);
        }
    }
}
