using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace Flow.App.Converters;

public class ConnectorColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is bool isConnected)
        {
            return new SolidColorBrush(Color.FromRgb(
                isConnected ? (byte)102 : (byte)51,
                isConnected ? (byte)102 : (byte)51,
                isConnected ? (byte)102 : (byte)51
            ));
        }
        return new SolidColorBrush(Color.FromRgb(51, 51, 51));
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
} 