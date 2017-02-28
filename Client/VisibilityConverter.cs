namespace HomeHub.Client
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Data;

    public class VisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return ((bool)value) ^ (((string)parameter) == "true") ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return (((Visibility)value) == Visibility.Visible ? true : false) ^ (((string)parameter) == "true");
        }
    }
}
