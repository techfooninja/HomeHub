namespace HomeHub.Client.Converters
{
    using System;
    using Windows.UI.Xaml.Data;

    public class DateTimeElapsedConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            DateTime time = (DateTime)value;
            var offset = DateTime.Now - time;

            if (offset.TotalSeconds < 60)
            {
                return " a moment ago";
            }

            if (offset.TotalMinutes < 60)
            {
                return String.Format(" {0} minute{1} ago", offset.TotalMinutes, offset.TotalMinutes == 1 ? String.Empty : "s");
            }

            if (offset.TotalHours < 24)
            {
                return String.Format(" {0} hour{1} ago", offset.TotalHours, offset.TotalHours == 1 ? String.Empty : "s");
            }
            
            return String.Format(" {0} day{1} ago", offset.TotalDays, offset.TotalDays == 1 ? String.Empty : "s");
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
