namespace HomeHub.Client.Converters
{
    using HomeHub.Shared;
    using System;
    using Windows.UI.Xaml.Data;

    public class TemperatureToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null)
            {
                return "---";
            }

            var temp = ((Temperature)value).ConvertToScale(ClientSettings.TemperatureFormat);
            return temp.Degrees.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if ((string)value == "---")
            {
                return null;
            }

            return new Temperature(ClientSettings.TemperatureFormat, float.Parse((string)value));
        }
    }
}
