using System;
using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Media;

namespace WeatherTeller.Converters;

public class BoolToBrushConverter : IValueConverter
{
    public IBrush TrueBrush { get; } = Brushes.Green;
    public IBrush FalseBrush { get; } = Brushes.DimGray;
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value switch
    {
        bool b => b ? TrueBrush : FalseBrush,
        _ => throw new ArgumentException("Value must be a boolean")
    };

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is Brush brush)
            return brush == TrueBrush;
        
        throw new ArgumentException("Value must be a Brush");
    }
}