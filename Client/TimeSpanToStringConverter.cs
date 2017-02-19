namespace HomeHub.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Windows.UI.Xaml.Data;

    public class TimeSpanToStringConverter : IValueConverter
    {
        private static TimeSpan _oneDay = new TimeSpan(1, 0, 0, 1);

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var time = (TimeSpan)value;
            if (time >= _oneDay || time == TimeSpan.MinValue)
            {
                return String.Empty;
            }

            return DateTime.Now.Date.Add(time).ToString("hh:mm tt");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
