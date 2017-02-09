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
            return ((Temperature)value).ConvertToScale(AppSettings.Instance.TemperatureFormat).Degrees;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return new Temperature(AppSettings.Instance.TemperatureFormat, float.Parse((string)value));
        }
    }
}
