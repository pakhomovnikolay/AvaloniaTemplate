using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Data.Converters;
using System;
using System.Globalization;

namespace AvaloniaTemplate.Infrastructures.Converters
{
    public class ColumnDefinitionGridLengthConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var gridLength = new GridLength();
            if (value is double size)
                gridLength = new GridLength(size, GridUnitType.Pixel);

            return gridLength;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => BindingOperations.DoNothing;
    }
}
