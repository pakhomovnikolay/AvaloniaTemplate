using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using System;
using System.Globalization;

namespace AvaloniaTemplate.Infrastructures.Converters
{
    public class ColorStringToSolidColorBrushConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            IBrush color = Brushes.Transparent;
            if (value is string brush)
                color = Helper.GetColor(brush);

            return color;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => BindingOperations.DoNothing;
    }
}
