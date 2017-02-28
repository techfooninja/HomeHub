namespace HomeHub.Client.Converters
{
    using Shared;
    using System;
    using Windows.UI.Xaml.Controls;
    using Windows.UI.Xaml.Data;

    public class RuleTypeSymbolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Type type = (Type)value;
            if (type == typeof(DefaultRule))
                return Symbol.Pin;
            else if (type == typeof(TemporaryOverrideRule))
                return Symbol.ReportHacked;
            else if (type == typeof(ScheduleRule))
                return Symbol.Clock;
            else
                return Symbol.Placeholder;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
