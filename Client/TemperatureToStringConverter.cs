namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Data;
    using HomeHub.Shared;

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
