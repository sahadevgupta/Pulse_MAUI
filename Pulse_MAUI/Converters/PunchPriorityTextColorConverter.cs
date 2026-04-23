using System;
using System.Globalization;

namespace Pulse_MAUI.Converters;

public class PunchPriorityTextColorConverter : IValueConverter
{
    private static readonly string[] LightBackgrounds = { "#CEE1EA", "#95A2D2", "#80C0E9" };

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var priority = value switch
        {
            int number => number,
            long number => (int)number,
            string text when int.TryParse(text, out var parsed) => parsed,
            _ => 0
        };

        var palette = new[]
        {
            "#04273F", "#4CA092", "#27AAE1", "#CEE1EA", "#8DC63F",
            "#09382B", "#5A3795", "#95A2D2", "#80C0E9"
        };

        var index = priority <= 0 ? 0 : (priority - 1) % palette.Length;
        var color = palette[index];
        return Color.FromArgb(LightBackgrounds.Contains(color, StringComparer.OrdinalIgnoreCase) ? "#04273F" : "#FFFFFF");
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
