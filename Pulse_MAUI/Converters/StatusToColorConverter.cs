using System;
using System.Globalization;

namespace Pulse_MAUI.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return Colors.Transparent;

        var status = value as string;
        return status switch
        {
            "Closed Complete" => Color.FromArgb("#5A3795"),
            "Pending" => Color.FromArgb("#27AAE1"),
            "In Progress" => Color.FromArgb("#4CA092"),
            _ => Color.FromArgb("#04273F")
        };

    }



    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


}