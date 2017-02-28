namespace HomeHub.Client.Converters
{
    using System;
    using Windows.UI.Text;
    using Windows.UI.Xaml.Data;

    public class BooleanToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool weight = (bool)value;
            return weight ? FontWeights.Bold : FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
