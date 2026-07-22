using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace AvaloniaTemplate.Infrastructures.Converters
{
    public class FontWeightBoldConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var fontWeight = FontWeight.Normal;
            if (value is bool isBold && isBold)
                fontWeight = FontWeight.Bold;

            return fontWeight;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => BindingOperations.DoNothing;
    }
}
