namespace HomeHub.Client
{
    using HomeHub.Shared;
    using System;
    using Windows.UI.Xaml.Data;

    public class TemperatureScaleToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return Enum.GetName(typeof(TemperatureScale), value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return Enum.Parse(typeof(TemperatureScale), (string)value);
        }
    }
}
