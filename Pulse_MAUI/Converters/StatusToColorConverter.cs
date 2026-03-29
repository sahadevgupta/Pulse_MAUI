using System;
using System.Globalization;

namespace Pulse_MAUI.Converters;

public class StatusToColorConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
            return Colors.Transparent;

        string status = value.ToString()?.Trim().ToLower();

        return status switch
        {
            "pending" => Application.Current.Resources["Gray300"],
            "in progress" => Application.Current.Resources["PrimaryBlue"],
            "closed complete" => Application.Current.Resources["PrimaryGreen"],
            _ => Application.Current.Resources["Gray300"]
        };
    }



    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }


}