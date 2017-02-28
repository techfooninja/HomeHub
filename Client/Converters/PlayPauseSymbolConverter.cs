namespace HomeHub.Client.Converters
{
    using System;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;

    public class PlayPauseSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Symbol.Play : Symbol.Pause;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
