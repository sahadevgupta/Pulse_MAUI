using System;
using System.Globalization;

namespace Pulse_MAUI.Converters;

public class PunchPriorityColorConverter : IValueConverter
{
    private static readonly string[] Palette =
    {
        "#04273F", "#4CA092", "#27AAE1", "#CEE1EA", "#8DC63F",
        "#09382B", "#5A3795", "#95A2D2", "#80C0E9"
    };

    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var priority = value switch
        {
            int number => number,
            long number => (int)number,
            string text when int.TryParse(text, out var parsed) => parsed,
            _ => 0
        };

        if (priority <= 0)
            return Color.FromArgb(Palette[0]);

        var index = (priority - 1) % Palette.Length;
        return Color.FromArgb(Palette[index]);
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
