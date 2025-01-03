using System;
using System.Globalization;
using Avalonia;
using Avalonia.Data.Converters;

namespace Flow.App.Converters;

public class PointToMarginConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Point point)
        {
            return new Thickness(point.X, point.Y, 0, 0);
        }
        return new Thickness();
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 