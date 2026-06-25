using Avalonia.Data.Converters;
using Avalonia.Layout;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace AvaloniaTemplate.Infrastructures.Converters
{
    public class Converter_CurrentTextAlignmentToBool : IMultiValueConverter
    {
        public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture)
        {
            var result = false;
            if (values is not null && values.Count == 3 && values[0] is Orientation position)
            {
                if (position == Orientation.Horizontal)
                {
                    if (values[1] is HorizontalAlignment HConfigAlignment && values[2] is HorizontalAlignment HCurrentAlignment)
                        result = HConfigAlignment == HCurrentAlignment;
                }
                else
                {
                    if (values[1] is VerticalAlignment VConfigAlignment && values[2] is VerticalAlignment VCurrentAlignment)
                        result = VConfigAlignment == VCurrentAlignment;
                }
            }
            return result;
        }

        public object? ConvertBack(IList<object?> value, Type targetType, object? parameter, CultureInfo culture)
            => throw new NotImplementedException();
    }
}
