namespace HomeHub.Client
{
    using System;
    using Windows.UI.Xaml.Data;

    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime today = DateTime.Now.Date;
            DateTime time = ((DateTimeOffset)value).DateTime;

            string date = String.Empty;

            if (today == time.Date)
            {
                date = "Today";
            }
            else if (today.AddDays(1) == time.Date)
            {
                date = "Tomorrow";
            }
            else if (today.AddDays(7) > time.Date)
            {
                date = time.DayOfWeek.ToString();
            }
            else
            {
                date = time.Date.ToString("d");
            }

            return String.Format("{0} at {1}", date, time.ToString("hh:mm tt"));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
