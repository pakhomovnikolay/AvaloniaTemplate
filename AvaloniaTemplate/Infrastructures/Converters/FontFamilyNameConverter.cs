using Avalonia.Data;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaTemplate.Infrastructures.Helpers;
using System;
using System.Globalization;
using System.Linq;

namespace AvaloniaTemplate.Infrastructures.Converters
{
    public class FontFamilyNameConverter : IValueConverter
    {
        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            var font = Helper.GetResource<FontFamily>("FontFamilyDefault");
            if (value is string fontFamily && !string.IsNullOrWhiteSpace(fontFamily))
                font = FontFamilyHelper.FontFamilies?.FirstOrDefault(x => x.Name.Equals(fontFamily)) ?? new FontFamily(fontFamily);

            return font ?? FontFamily.Default;
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
            => BindingOperations.DoNothing;
    }
}
